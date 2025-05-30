using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class PawnTests
{
    private readonly ChessBoard _board = new();

    [Fact]
    public void Pawn_CanMoveOneSquareForward()
    {
        var pawn = new Pawn(PieceColour.White);
        var move = new ChessMove((6, 0), (5, 0));

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void Pawn_CanMoveTwoSquaresOnFirstMove()
    {
        var pawn = new Pawn(PieceColour.White);
        var move = new ChessMove((6, 0), (4, 0));

        pawn.ValidMove(move, _board).Should().BeTrue();
    }

    [Fact]
    public void Pawn_CannotMoveBackwards()
    {
        var pawn = new Pawn(PieceColour.White);
        var move = new ChessMove((6, 0), (7, 0));
        
        pawn.ValidMove(move, _board).Should().BeFalse();
    }
}