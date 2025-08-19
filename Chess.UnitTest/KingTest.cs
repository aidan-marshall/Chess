using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class KingTest
{
    private readonly IChessBoard _board;

    public KingTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    [Theory]
    [InlineData(3, 3, 2, 2)] // Up-Left
    [InlineData(3, 3, 2, 3)] // Up
    [InlineData(3, 3, 2, 4)] // Up-Right
    [InlineData(3, 3, 3, 2)] // Left
    [InlineData(3, 3, 3, 4)] // Right
    [InlineData(3, 3, 4, 2)] // Down-Left
    [InlineData(3, 3, 4, 3)] // Down
    [InlineData(3, 3, 4, 4)] // Down-Right
    public void ValidMove_ShouldSucceed_WhenMovingOneSquareToEmpty(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var king = new King(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), king);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = king.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var king = new King(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((3, 3), king);
        _board.SetPiece((4, 4), enemyPawn);
        var move = new ChessMove((3, 3), (4, 4));

        // Act
        bool isValid = king.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(7, 4, 7, 6)] // White King-side castle (e1g1)
    [InlineData(7, 4, 7, 2)] // White Queen-side castle (e1c1)
    [InlineData(0, 4, 0, 6)] // Black King-side castle (e8g8)
    [InlineData(0, 4, 0, 2)] // Black Queen-side castle (e8c8)
    public void ValidMove_ShouldSucceed_WhenIdentifyingCastlePattern(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        // This test ONLY validates that the King piece recognizes the 2-square move pattern.
        // It does NOT test the rules of castling (e.g., clear path, rights),
        // as that is the ChessGame's responsibility.

        // Create a king of the correct color based on its starting row.
        var kingColour = (fromRow == 7) ? PieceColour.White : PieceColour.Black;
        var king = new King(kingColour);

        _board.SetPiece((fromRow, fromCol), king);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = king.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue("the king piece should identify a two-square horizontal move from its starting square as a castling attempt");
    }
    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var king = new King(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece((3, 3), king);
        _board.SetPiece((4, 4), friendlyPawn);
        var move = new ChessMove((3, 3), (4, 4));

        // Act
        bool isValid = king.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a king cannot capture a friendly piece");
    }

    [Theory]
    [InlineData(3, 3, 3, 5)] // Two squares straight
    [InlineData(3, 3, 5, 5)] // Two squares diagonally
    [InlineData(3, 3, 1, 4)] // Knight-like move
    public void InvalidMove_ShouldFail_WhenMovePatternIsInvalid(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var king = new King(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), king);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = king.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse();
    }
}
