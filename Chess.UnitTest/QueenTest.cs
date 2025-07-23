using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class QueenTest
{
    private readonly ChessBoard _board;

    public QueenTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    [Theory]
    [InlineData(3, 3, 3, 7)] // Move right (Rook-like)
    [InlineData(3, 3, 7, 3)] // Move down (Rook-like)
    [InlineData(3, 3, 0, 0)] // Move up-left (Bishop-like)
    [InlineData(3, 3, 6, 6)] // Move down-right (Bishop-like)
    public void ValidMove_ShouldSucceed_WhenMovingInAnyValidDirection(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), queen);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((3, 3), queen);
        _board.SetPiece((7, 7), enemyPawn);
        var move = new ChessMove((3, 3), (7, 7));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedDiagonally()
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((2, 2), queen);
        _board.SetPiece((4, 4), blockingPawn); // Blocker is between start and end
        var move = new ChessMove((2, 2), (6, 6));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the diagonal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedHorizontally()
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.White); // Blocker can be friendly
        _board.SetPiece((3, 1), queen);
        _board.SetPiece((3, 4), blockingPawn); // Blocker is between start and end
        var move = new ChessMove((3, 1), (3, 6));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the horizontal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece((3, 3), queen);
        _board.SetPiece((3, 7), friendlyPawn);
        var move = new ChessMove((3, 3), (3, 7));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece cannot capture a friendly piece");
    }

    [Theory]
    [InlineData(3, 3, 1, 2)] // Invalid Knight-move
    [InlineData(3, 3, 5, 4)] // another invalid Knight-move
    public void InvalidMove_ShouldFail_WhenMovePatternIsInvalid(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var queen = new Queen(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), queen);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = queen.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a queen cannot move like a knight");
    }
}