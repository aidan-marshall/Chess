using Chess.Engine.Board;
using Chess.Engine.Game;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Moq;

namespace Chess.Engine.UnitTest.GameTests;

public class ChessGameFenTests
{

    public ChessGameFenTests()
    {
    }

    [Fact]
    public void ToFen_NewGame_ReturnsStartingPositionFen()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);

        // Act
        var fen = game.ToFen();

        // Assert
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", fen);
    }

    [Fact]
    public void ToFen_AfterOneMove_ReturnsCorrectFen()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        
        // Make e4
        var move = Move.Of(6, 4, 4, 4);
        game.TryMakeMove(move, PieceColour.White);

        // Act
        var fen = game.ToFen();

        // Assert
        Assert.Equal("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", fen);
    }

    [Fact]
    public void ToFen_AfterTwoMoves_ReturnsCorrectFullMoveNumber()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        
        // Make e4 e5
        game.TryMakeMove(Move.Of(6, 4, 4, 4), PieceColour.White);
        game.TryMakeMove(Move.Of(1, 4, 3, 4), PieceColour.Black);

        // Act
        var fen = game.ToFen();

        // Assert
        Assert.Equal("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2", fen);
    }

    [Fact]
    public void LoadFromFen_StartingPosition_LoadsSuccessfully()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal(PieceColour.White, game.ToMove);
        Assert.Equal(GameState.Ongoing, game.State);
    }

    [Fact]
    public void LoadFromFen_AfterE4_LoadsWithCorrectState()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal(PieceColour.Black, game.ToMove);
        Assert.Equal(GameState.Ongoing, game.State);
        Assert.Empty(game.MoveHistory); // Move history should be empty after FEN load
    }

    [Fact]
    public void LoadFromFen_CheckPosition_DetectsCheck()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        // White king in check from black queen
        var fen = "4k3/8/8/8/8/8/8/4K2q w - - 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal(GameState.Check, game.State);
    }

    [Fact]
    public void LoadFromFen_CheckmatePosition_DetectsCheckmate()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        // Scholar's mate position
        var fen = "r1bqkb1r/pppp1Qpp/2n2n2/4p3/2B1P3/8/PPPP1PPP/RNB1K1NR b KQkq - 0 4";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal(GameState.Checkmate, game.State);
    }

    [Fact]
    public void LoadFromFen_StalematePosition_DetectsStalemate()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "7k/5Q2/6K1/8/8/8/8/8 b - - 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        Assert.Equal(GameState.Stalemate, game.State);
        Assert.Equal(DrawReason.StaleMate, game.DrawReason);
    }

    [Fact]
    public void LoadFromFen_InvalidFen_ReturnsFalseWithError()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var invalidFen = "invalid fen string";

        // Act
        var success = game.LoadFromFen(invalidFen, out var error);

        // Assert
        Assert.False(success);
        Assert.NotNull(error);
        Assert.Contains("Invalid FEN", error);
    }

    [Fact]
    public void LoadFromFen_ClearsPreviousGameState()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        
        // Play a move
        game.TryMakeMove(Move.Of(6, 4, 4, 4), PieceColour.White);
        Assert.Single(game.MoveHistory);

        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Empty(game.MoveHistory); // Should clear previous moves
    }

    [Fact]
    public void FromFen_ValidFen_CreatesNewGame()
    {
        // Arrange
        var fen = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2";

        // Act
        var game = ChessGame.FromFen(fen, out var error);

        // Assert
        Assert.NotNull(game);
        Assert.Null(error);
        Assert.Equal(PieceColour.White, game.ToMove);
    }

    [Fact]
    public void FromFen_InvalidFen_ReturnsNull()
    {
        // Arrange
        var invalidFen = "invalid";

        // Act
        var game = ChessGame.FromFen(invalidFen, out var error);

        // Assert
        Assert.Null(game);
        Assert.NotNull(error);
    }

    [Fact]
    public void FenRoundTrip_AfterMultipleMoves_PreservesPosition()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        
        // Play some moves
        game.TryMakeMove(Move.Of(6, 4, 4, 4), PieceColour.White); // e4
        game.TryMakeMove(Move.Of(1, 4, 3, 4), PieceColour.Black); // e5
        game.TryMakeMove(Move.Of(7, 6, 5, 5), PieceColour.White); // Nf3

        // Act - Export to FEN
        var fen = game.ToFen();

        // Create new game from FEN
        var newGame = ChessGame.FromFen(fen, out var error);
        Assert.NotNull(newGame);

        var newFen = newGame.ToFen();

        // Assert - FEN should match
        Assert.Equal(fen, newFen);
    }

    [Fact]
    public void LoadFromFen_WithHalfMoveClock_PreservesValue()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 25 10";

        // Act
        game.LoadFromFen(fen, out _);
        var exportedFen = game.ToFen();

        // Assert
        Assert.Contains(" 25 ", exportedFen); // Half move clock preserved
    }

    [Fact]
    public void LoadFromFen_ThenMakeMoves_ContinuesCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
        
        game.LoadFromFen(fen, out _);

        // Act - Make e5
        var result = game.TryMakeMove(Move.Of(1, 4, 3, 4), PieceColour.Black);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(PieceColour.White, game.ToMove);
        Assert.Single(game.MoveHistory); // Should have one move after loading from FEN
    }

    [Fact]
    public void LoadFromFen_PositionWithNoCastlingRights_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        var fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w - - 0 1";

        // Act
        game.LoadFromFen(fen, out _);
        var exportedFen = game.ToFen();

        // Assert
        Assert.Contains(" - ", exportedFen); // No castling rights
    }

    [Fact]
    public void LoadFromFen_InsufficientMaterial_DetectsDraw()
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);
        // King vs King
        var fen = "8/8/8/4k3/8/8/8/4K3 w - - 0 1";

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success);
        Assert.Equal(GameState.Draw, game.State);
        Assert.Equal(DrawReason.InsufficientMaterial, game.DrawReason);
    }

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1")]
    [InlineData("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1")]
    [InlineData("8/8/8/4k3/8/8/4P3/4K3 w - - 0 1")]
    public void LoadFromFen_VariousValidPositions_LoadsSuccessfully(string fen)
    {
        // Arrange
        var board = new ChessBoard();
        var game = new ChessGame(board);

        // Act
        var success = game.LoadFromFen(fen, out var error);

        // Assert
        Assert.True(success, $"Failed to load FEN: {error}");
        Assert.Null(error);
    }
}