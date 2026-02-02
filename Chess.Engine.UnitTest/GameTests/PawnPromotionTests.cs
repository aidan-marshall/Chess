using Chess.Engine.Board;
using Chess.Engine.Game;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.UnitTest.Helpers;

namespace Chess.Engine.UnitTest.GameTests;

public class PawnPromotionTests
{
    [Fact]
    public void TryMakeMove_PawnReachesPromotionRank_StateIsPromotionPending()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4); // e7-e8
        
        // Act
        var result = game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.PromotionPending, game.State);
        Assert.NotNull(result.ValidationResult);
        Assert.Equal(Validation.SpecialMoveType.Promotion, result.ValidationResult.SpecialMoveType);
    }


    [Fact]
    public void CompletePromotion_WithQueen_ReplacesPawnWithQueen()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act
        var result = game.CompletePromotion(PieceType.Queen);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(PieceType.Queen, result.PromotedToPieceType);
        Assert.NotEqual(GameState.PromotionPending, game.State);
    }

    [Theory]
    [InlineData(PieceType.Queen)]
    [InlineData(PieceType.Rook)]
    [InlineData(PieceType.Bishop)]
    [InlineData(PieceType.Knight)]
    public void CompletePromotion_WithValidPieceType_Succeeds(PieceType promotionType)
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act
        var result = game.CompletePromotion(promotionType);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(promotionType, result.PromotedToPieceType);
    }

    [Fact]
    public void CompletePromotion_WithPawnType_ThrowsArgumentException()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => 
            game.CompletePromotion(PieceType.Pawn));
        
        Assert.Contains("Cannot promote pawn to Pawn", ex.Message);
    }

    [Fact]
    public void CompletePromotion_WithKingType_ThrowsArgumentException()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            game.CompletePromotion(PieceType.King));
        
        Assert.Contains("Cannot promote pawn to King", ex.Message);
    }

    [Fact]
    public void CompletePromotion_WhenNotPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var board = new ChessBoard(); // Standard starting position
        var game = new ChessGame(board);
        
        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            game.CompletePromotion(PieceType.Queen));
        
        Assert.Contains("no promotion is pending", ex.Message);
    }

    [Fact]
    public void TryMakeMove_PromotionWithCapture_CapturedPieceIsRecorded()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithPromotionCapture();
        var game = new ChessGame(board);
        var capturePromotionMove = Move.Of(1, 4, 0, 3); // e7xd8
        
        // Act
        var result = game.TryMakeMove(capturePromotionMove, PieceColour.White);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.PromotionPending, game.State);
        Assert.NotNull(result.CapturedPiece);
        Assert.Equal(PieceColour.Black, result.CapturedPiece.Colour);
    }

    [Fact]
    public void CompletePromotion_ResultsInCheck_StateIsCheck()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithPromotionCheck();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act
        var result = game.CompletePromotion(PieceType.Queen);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.Check, result.NewGameState);
    }

    [Fact]
    public void CompletePromotion_CompletesSuccessfully_TurnChangesToOpponent()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        
        Assert.Equal(PieceColour.White, game.ToMove);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // White played, so it should be Black's turn
        Assert.Equal(PieceColour.Black, game.ToMove);
        
        // Act
        game.CompletePromotion(PieceType.Queen);
        
        // Assert
        // Turn doesn't change during promotion, it already changed during TryMakeMove
        Assert.Equal(PieceColour.Black, game.ToMove);
    }
}
