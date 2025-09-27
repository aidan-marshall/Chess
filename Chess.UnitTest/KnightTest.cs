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

    public static IEnumerable<object[]> ValidKnightMoves =>
        new List<object[]>
        {
            new object[] { new Position(4, 3), new Position(2, 2) }, // Up 2, Left 1
            new object[] { new Position(4, 3), new Position(2, 4) }, // Up 2, Right 1
            new object[] { new Position(4, 3), new Position(6, 2) }, // Down 2, Left 1
            new object[] { new Position(4, 3), new Position(6, 4) }, // Down 2, Right 1
            new object[] { new Position(4, 3), new Position(3, 1) }, // Up 1, Left 2
            new object[] { new Position(4, 3), new Position(3, 5) }, // Up 1, Right 2
            new object[] { new Position(4, 3), new Position(5, 1) }, // Down 1, Left 2
            new object[] { new Position(4, 3), new Position(5, 5) }  // Down 1, Right 2
        };

    [Theory]
    [MemberData(nameof(ValidKnightMoves))]
    public void ValidMove_ShouldSucceed_WhenMovingInLShapeToEmptySquare(Position from, Position to)
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece(from, knight);
        var move = new Move(from, to);

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
        var from = new Position(4, 4);
        var to = new Position(2, 5);

        _board.SetPiece(from, knight);
        _board.SetPiece(to, enemyPawn);
        var move = new Move(from, to);

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> ValidKnightMovesFromCorner =>
        new List<object[]>
        {
            new object[] { new Position(7, 0), new Position(5, 1) }, // From a1 to c2
            new object[] { new Position(7, 0), new Position(6, 2) }  // From a1 to b3
        };

    [Theory]
    [MemberData(nameof(ValidKnightMovesFromCorner))]
    public void ValidMove_ShouldSucceed_WhenMovingFromCorner(Position from, Position to)
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece(from, knight);
        var move = new Move(from, to);

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
        var from = new Position(4, 4);
        var to = new Position(2, 5);

        _board.SetPiece(from, knight);
        _board.SetPiece(to, friendlyPawn);
        var move = new Move(from, to);

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a knight cannot capture a friendly piece");
    }

    public static IEnumerable<object[]> InvalidKnightMoves =>
        new List<object[]>
        {
            new object[] { new Position(4, 4), new Position(5, 5) }, // Invalid Diagonal
            new object[] { new Position(4, 4), new Position(4, 6) }, // Invalid Straight
            new object[] { new Position(4, 4), new Position(1, 1) }  // Invalid Long-Range
        };

    [Theory]
    [MemberData(nameof(InvalidKnightMoves))]
    public void InvalidMove_ShouldFail_WhenMovePatternIsNotLShape(Position from, Position to)
    {
        // Arrange
        var knight = new Knight(PieceColour.White);
        _board.SetPiece(from, knight);
        var move = new Move(from, to);

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
        var from = new Position(4, 4);
        _board.SetPiece(from, knight);
        var move = new Move(from, from);

        // Act
        bool isValid = knight.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece must move to a different square");
    }
}
