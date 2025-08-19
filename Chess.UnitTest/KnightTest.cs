using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class KnightTest
{
    private readonly IChessBoard _board;

    public KnightTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    [Theory]
    [InlineData(4, 3, 2, 2)] // Up 2, Left 1
    [InlineData(4, 3, 2, 4)] // Up 2, Right 1
    [InlineData(4, 3, 6, 2)] // Down 2, Left 1
    [InlineData(4, 3, 6, 4)] // Down 2, Right 1
    [InlineData(4, 3, 3, 1)] // Up 1, Left 2
    [InlineData(4, 3, 3, 5)] // Up 1, Right 2
    [InlineData(4, 3, 5, 1)] // Down 1, Left 2
    [InlineData(4, 3, 5, 5)] // Down 1, Right 2
    public void ValidMove_ShouldSucceed_WhenMovingInLShapeToEmptySquare(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), knight);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((4, 4), knight);
        _board.SetPiece((2, 5), enemyPawn);
        var move = new ChessMove((4, 4), (2, 5));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(7, 0, 5, 1)] // From a1 to c2
    [InlineData(7, 0, 6, 2)] // From a1 to b3
    public void ValidMove_ShouldSucceed_WhenMovingFromCorner(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange: Test from corner a1 (row 7, col 0)
        var knight = new Knight(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), knight);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece((4, 4), knight);
        _board.SetPiece((2, 5), friendlyPawn);
        var move = new ChessMove((4, 4), (2, 5));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a knight cannot capture a friendly piece");
    }

    [Theory]
    [InlineData(4, 4, 5, 5)] // Invalid Diagonal
    [InlineData(4, 4, 4, 6)] // Invalid Straight
    [InlineData(4, 4, 1, 1)] // Invalid Long-Range
    public void InvalidMove_ShouldFail_WhenMovePatternIsNotLShape(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), knight);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenNotMoving()
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece((4, 4), knight);
        var move = new ChessMove((4, 4), (4, 4));

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece must move to a different square");
    }
}