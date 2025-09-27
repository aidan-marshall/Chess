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
        var from = new Position(fromRow, fromCol);
        var to = new Position(toRow, toCol);

        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece(from, bishop);
        var move = new Move(from, to);

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var from = new Position(2, 2);
        var to = new Position(5, 5);

        var bishop = new Bishop(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece(from, bishop);
        _board.SetPiece(to, enemyPawn);
        var move = new Move(from, to);

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenMovingFromCorner()
    {
        // Arrange: from corner a1 (row 7, col 0) to h8 (row 0, col 7)
        var from = new Position(7, 0);
        var to = new Position(0, 7);

        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece(from, bishop);
        var move = new Move(from, to);

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
        var from = new Position(2, 2);
        var block = new Position(4, 4);
        var to = new Position(6, 6);

        var bishop = new Bishop(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black); // Blocker can be friend or foe
        _board.SetPiece(from, bishop);
        _board.SetPiece(block, blockingPawn);
        var move = new Move(from, to);

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the path between (2,2) and (6,6) is blocked by a piece at (4,4)");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var from = new Position(2, 2);
        var to = new Position(5, 5);

        var bishop = new Bishop(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece(from, bishop);
        _board.SetPiece(to, friendlyPawn);
        var move = new Move(from, to);

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
        var from = new Position(fromRow, fromCol);
        var to = new Position(toRow, toCol);

        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece(from, bishop);
        var move = new Move(from, to);

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenNotMoving()
    {
        // Arrange
        var from = new Position(3, 3);
        var to = new Position(3, 3);

        var bishop = new Bishop(PieceColour.White);
        _board.SetPiece(from, bishop);
        var move = new Move(from, to);

        // Act
        bool isValid = bishop.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece must move to a different square");
    }
}