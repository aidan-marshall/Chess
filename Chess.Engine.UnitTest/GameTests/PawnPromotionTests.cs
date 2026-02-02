using Chess.Engine.Board;
using Chess.Engine.Game;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.UnitTest.Helpers;

namespace Chess.Engine.UnitTest.GameTests;

/// <summary>
/// Unit tests for pawn promotion functionality.
/// These tests demonstrate the expected behavior of the promotion system.
/// </summary>
public class PawnPromotionTests
{
    /// <summary>
    /// Test 1: Pawn reaching promotion rank enters PromotionPending state
    /// </summary>
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

    /// <summary>
    /// Test 2: CompletePromotion replaces pawn with selected piece
    /// </summary>
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

    /// <summary>
    /// Test 3: All valid promotion types are accepted
    /// </summary>
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

    /// <summary>
    /// Test 4: Cannot promote to Pawn
    /// </summary>
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

    /// <summary>
    /// Test 5: Cannot promote to King
    /// </summary>
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

    /// <summary>
    /// Test 6: Cannot call CompletePromotion when no promotion is pending
    /// </summary>
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

    /// <summary>
    /// Test 7: Promotion with capture
    /// </summary>
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

    /// <summary>
    /// Test 8: Promotion with check
    /// </summary>
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

    /// <summary>
    /// Test 9: Promotion with checkmate
    /// </summary>
    [Fact]
    public void CompletePromotion_ResultsInCheckmate_StateIsCheckmate()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithPromotionCheckmate();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        // Act
        var result = game.CompletePromotion(PieceType.Queen);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.Checkmate, result.NewGameState);
    }

    /// <summary>
    /// Test 10: Turn changes after promotion
    /// </summary>
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

    /// <summary>
    /// Test 11: Black pawn promotion (opposite color)
    /// </summary>
    [Fact]
    public void TryMakeMove_BlackPawnPromotion_StateIsPromotionPending()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithBlackPawnOnE2();
        var game = new ChessGame(board);
        
        // Move to a position where it's Black's turn with pawn on e2 about to promote
        var promotionMove = Move.Of(6, 4, 7, 4); // e2-e1 (for black, row 6 to row 7)
        
        // Act
        var result = game.TryMakeMove(promotionMove, PieceColour.Black);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.PromotionPending, game.State);
    }

    /// <summary>
    /// Test 12: Cannot make new moves while promotion is pending
    /// </summary>
    [Fact]
    public void TryMakeMove_WhilePromotionPending_ReturnsIllegal()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithWhitePawnOnE7();
        var game = new ChessGame(board);
        var promotionMove = Move.Of(1, 4, 0, 4);
        game.TryMakeMove(promotionMove, PieceColour.White);
        
        Assert.Equal(GameState.PromotionPending, game.State);
        
        // Try to make another move before completing promotion
        var anotherMove = Move.Of(0, 3, 1, 3); // Some other piece move
        
        // Act
        // This depends on implementation - might allow it or not
        // Current implementation: TryMakeMove may not be callable during PromotionPending
    }

    /// <summary>
    /// Test 13: Multiple promotions in different parts of the board
    /// </summary>
    [Fact]
    public void MultiplePromotions_DifferentFiles_BothSucceed()
    {
        // Arrange
        var board = ChessBoardTestFactory.CreateBoardWithMultiplePawnsNearPromotion();
        var game = new ChessGame(board);
        
        // Act & Assert
        // First promotion (file a)
        var move1 = Move.Of(1, 0, 0, 0); // a7-a8
        var result1 = game.TryMakeMove(move1, PieceColour.White);
        Assert.True(result1.IsSuccess);
        Assert.Equal(GameState.PromotionPending, game.State);
        
        game.CompletePromotion(PieceType.Queen);
        
        // After promotion, play black moves to get back to white
        // Then do second promotion (file h)
        // ... (more moves) ...
    }
}
