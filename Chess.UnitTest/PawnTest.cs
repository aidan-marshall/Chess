using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class PawnTest
{
    private readonly IChessBoard _board;

    public PawnTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. WHITE PAWN Tests ---

    public static TheoryData<Position, Position> WhitePawnSingleMove =>
        new()
        {
            { new Position(6, 4), new Position(5, 4) } // e2 → e3
        };

    [Theory]
    [MemberData(nameof(WhitePawnSingleMove))]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenMovingOneSquareForward(Position from, Position to)
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece(from, pawn);
        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    public static TheoryData<Position, Position> WhitePawnDoubleMove =>
        new()
        {
            { new Position(6, 4), new Position(4, 4) } // e2 → e4
        };

    [Theory]
    [MemberData(nameof(WhitePawnDoubleMove))]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenMovingTwoSquaresForwardOnFirstMove(Position from, Position to)
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece(from, pawn);
        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_InvalidMove_ShouldFail_WhenMovingTwoSquaresNotOnFirstMove()
    {
        var pawn = new Pawn(PieceColour.White);
        var from = new Position(5, 4); // e3
        var to = new Position(3, 4);   // e5
        _board.SetPiece(from, pawn);

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_InvalidMove_ShouldFail_WhenMovingForwardAndPathIsBlocked()
    {
        var pawn = new Pawn(PieceColour.White);
        var from = new Position(6, 4); // e2
        var to = new Position(5, 4);   // e3
        _board.SetPiece(from, pawn);
        _board.SetPiece(to, new Pawn(PieceColour.Black)); // Blocker

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeFalse("cannot move forward onto an occupied square");
    }

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenCapturingDiagonally()
    {
        var pawn = new Pawn(PieceColour.White);
        var from = new Position(5, 4); // e3
        var to = new Position(4, 5);   // f4
        _board.SetPiece(from, pawn);
        _board.SetPiece(to, new Pawn(PieceColour.Black));

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenPerformingEnPassant()
    {
        var pawn = new Pawn(PieceColour.White);
        var from = new Position(3, 4); // e5
        var to = new Position(2, 3);   // d6
        _board.SetPiece(from, pawn);

        // Opponent pawn just moved from d7 → d5
        _board.EnPassantTargetSquare = to;

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    // --- II. BLACK PAWN Tests ---

    public static TheoryData<Position, Position> BlackPawnSingleMove =>
        new()
        {
            { new Position(1, 4), new Position(2, 4) } // e7 → e6
        };

    [Theory]
    [MemberData(nameof(BlackPawnSingleMove))]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenMovingOneSquareForward(Position from, Position to)
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece(from, pawn);
        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    public static TheoryData<Position, Position> BlackPawnDoubleMove =>
        new()
        {
            { new Position(1, 4), new Position(3, 4) } // e7 → e5
        };

    [Theory]
    [MemberData(nameof(BlackPawnDoubleMove))]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenMovingTwoSquaresForwardOnFirstMove(Position from, Position to)
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece(from, pawn);
        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_InvalidMove_ShouldFail_WhenMovingForwardAndPathIsBlocked()
    {
        var pawn = new Pawn(PieceColour.Black);
        var from = new Position(1, 4); // e7
        var to = new Position(3, 4);   // e5
        _board.SetPiece(from, pawn);
        _board.SetPiece(new Position(2, 4), new Pawn(PieceColour.White)); // Blocker on e6

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeFalse("two-square move is blocked");
    }

    [Fact]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenCapturingDiagonally()
    {
        var pawn = new Pawn(PieceColour.Black);
        var from = new Position(2, 4); // e6
        var to = new Position(3, 3);   // d5
        _board.SetPiece(from, pawn);
        _board.SetPiece(to, new Pawn(PieceColour.White));

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_InvalidMove_ShouldFail_WhenMovingDiagonallyToEmptySquare()
    {
        var pawn = new Pawn(PieceColour.Black);
        var from = new Position(2, 4); // e6
        var to = new Position(3, 3);   // d5 (empty)
        _board.SetPiece(from, pawn);

        var move = new Move(from, to);

        pawn.ValidMove(move, _board).Should().BeFalse();
    }
}
