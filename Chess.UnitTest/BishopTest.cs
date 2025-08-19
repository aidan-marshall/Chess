using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class BishopTest
{
    private readonly IChessBoard _board;

    public BishopTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    [Theory]
    [InlineData(3, 3, 5, 5)] // Down-Right
    [InlineData(3, 3, 1, 5)] // Up-Right
    [InlineData(3, 3, 5, 1)] // Down-Left
    [InlineData(3, 3, 1, 1)] // Up-Left
    public void ValidMove_ShouldSucceed_WhenMovingDiagonallyOnEmptyBoard(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), bishop);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((2, 2), bishop);
        _board.SetPiece((5, 5), enemyPawn);
        var move = new ChessMove((2, 2), (5, 5));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenMovingFromCorner()
    {
        // Arrange: Test from corner a1 (row 7, col 0)
        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece((7, 0), bishop);
        var move = new ChessMove((7, 0), (0, 7)); // Move to h8

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlocked()
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black); // Blocker can be friend or foe
        _board.SetPiece((2, 2), bishop);
        _board.SetPiece((4, 4), blockingPawn); // Place a piece in the middle of the path
        var move = new ChessMove((2, 2), (6, 6));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the path between (2,2) and (6,6) is blocked by a piece at (4,4)");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece((2, 2), bishop);
        _board.SetPiece((5, 5), friendlyPawn);
        var move = new ChessMove((2, 2), (5, 5));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece cannot capture another piece of the same color");
    }

    [Theory]
    [InlineData(3, 3, 3, 6)] // Invalid Straight (Horizontal)
    [InlineData(3, 3, 6, 3)] // Invalid Straight (Vertical)
    [InlineData(3, 3, 4, 5)] // Invalid Knight-move
    public void InvalidMove_ShouldFail_WhenMovePatternIsNotDiagonal(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), bishop);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenNotMoving()
    {
        // Arrange
        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece((3, 3), bishop);
        var move = new ChessMove((3, 3), (3, 3));

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece must move to a different square");
    }
}