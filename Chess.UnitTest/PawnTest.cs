using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class PawnTest
{
    private readonly ChessBoard _board;

    public PawnTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }


    // --- I. WHITE PAWN Tests ---

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenMovingOneSquareForward()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((6, 4), pawn); // e2
        var move = new ChessMove((6, 4), (5, 4)); // to e3

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenMovingTwoSquaresForwardOnFirstMove()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((6, 4), pawn); // e2
        var move = new ChessMove((6, 4), (4, 4)); // to e4

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_InvalidMove_ShouldFail_WhenMovingTwoSquaresNotOnFirstMove()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((5, 4), pawn); // e3
        var move = new ChessMove((5, 4), (3, 4)); // to e5

        pawn.ValidMove(move, _board).Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_InvalidMove_ShouldFail_WhenMovingForwardAndPathIsBlocked()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((6, 4), pawn); // e2
        _board.SetPiece((5, 4), new Pawn(PieceColour.Black)); // Blocker on e3
        var move = new ChessMove((6, 4), (5, 4));

        pawn.ValidMove(move, _board).Should().BeFalse("cannot move forward onto an occupied square");
    }

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenCapturingDiagonally()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((5, 4), pawn); // e3
        _board.SetPiece((4, 5), new Pawn(PieceColour.Black)); // Enemy on f4
        var move = new ChessMove((5, 4), (4, 5));

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_ValidMove_ShouldSucceed_WhenPerformingEnPassant()
    {
        var pawn = new Pawn(PieceColour.White);
        _board.SetPiece((3, 4), pawn); // White pawn on e5
                                       // The opponent's pawn on d7 just moved to d5, setting the en passant target.
        _board.EnPassantTargetSquare = (2, 3); // En passant target is d6

        var move = new ChessMove((3, 4), (2, 3)); // White pawn captures en passant to d6

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    // --- II. BLACK PAWN Tests ---

    [Fact]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenMovingOneSquareForward()
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece((1, 4), pawn); // e7
        var move = new ChessMove((1, 4), (2, 4)); // to e6

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenMovingTwoSquaresForwardOnFirstMove()
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece((1, 4), pawn); // e7
        var move = new ChessMove((1, 4), (3, 4)); // to e5

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_InvalidMove_ShouldFail_WhenMovingForwardAndPathIsBlocked()
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece((1, 4), pawn); // e7
        _board.SetPiece((2, 4), new Pawn(PieceColour.White)); // Blocker on e6
        var move = new ChessMove((1, 4), (3, 4)); // Attempt to jump to e5

        pawn.ValidMove(move, _board).Should().BeFalse("two-square move is blocked");
    }

    [Fact]
    public void BlackPawn_ValidMove_ShouldSucceed_WhenCapturingDiagonally()
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece((2, 4), pawn); // e6
        _board.SetPiece((3, 3), new Pawn(PieceColour.White)); // Enemy on d5
        var move = new ChessMove((2, 4), (3, 3));

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_InvalidMove_ShouldFail_WhenMovingDiagonallyToEmptySquare()
    {
        var pawn = new Pawn(PieceColour.Black);
        _board.SetPiece((2, 4), pawn); // e6
        var move = new ChessMove((2, 4), (3, 3)); // to d5, which is empty and not en-passant target

        pawn.ValidMove(move, _board).Should().BeFalse();
    }
}