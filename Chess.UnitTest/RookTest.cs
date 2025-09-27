using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class RookTest
{
    private readonly IChessBoard _board;

    public RookTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    public static IEnumerable<object[]> RookValidHorizontalMoves =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(3, 7) }, // move right
            new object[] { new Position(3, 3), new Position(3, 0) }  // move left
        };

    [Theory]
    [MemberData(nameof(RookValidHorizontalMoves))]
    public void ValidMove_ShouldSucceed_WhenMovingHorizontallyOnEmptyBoard(Position from, Position to)
    {
        var rook = new Rook(PieceColour.White);
        _board.SetPiece(from, rook);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeTrue();
    }

    public static IEnumerable<object[]> RookValidVerticalMoves =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(0, 3) }, // move up
            new object[] { new Position(3, 3), new Position(7, 3) }  // move down
        };

    [Theory]
    [MemberData(nameof(RookValidVerticalMoves))]
    public void ValidMove_ShouldSucceed_WhenMovingVerticallyOnEmptyBoard(Position from, Position to)
    {
        var rook = new Rook(PieceColour.White);
        _board.SetPiece(from, rook);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        var rook = new Rook(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);

        var from = new Position(3, 3);
        var to = new Position(3, 7);

        _board.SetPiece(from, rook);
        _board.SetPiece(to, enemyPawn);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedHorizontally()
    {
        var rook = new Rook(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black);

        var from = new Position(3, 3);
        var to = new Position(3, 7);
        var blocker = new Position(3, 5);

        _board.SetPiece(from, rook);
        _board.SetPiece(blocker, blockingPawn);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeFalse("the horizontal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedVertically()
    {
        var rook = new Rook(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.White);

        var from = new Position(3, 3);
        var to = new Position(7, 3);
        var blocker = new Position(5, 3);

        _board.SetPiece(from, rook);
        _board.SetPiece(blocker, blockingPawn);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeFalse("the vertical path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        var rook = new Rook(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);

        var from = new Position(3, 3);
        var to = new Position(3, 7);

        _board.SetPiece(from, rook);
        _board.SetPiece(to, friendlyPawn);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeFalse("a piece cannot capture a friendly piece");
    }

    public static IEnumerable<object[]> RookInvalidMoves =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(5, 5) }, // diagonal
            new object[] { new Position(3, 3), new Position(4, 5) }  // knight-like
        };

    [Theory]
    [MemberData(nameof(RookInvalidMoves))]
    public void InvalidMove_ShouldFail_WhenMovePatternIsNotStraight(Position from, Position to)
    {
        var rook = new Rook(PieceColour.White);
        _board.SetPiece(from, rook);

        var move = new Move(from, to);
        rook.ValidMove(move, _board).Should().BeFalse("a rook can only move straight horizontally or vertically");
    }
}
