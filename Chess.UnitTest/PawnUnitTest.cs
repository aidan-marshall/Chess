using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class PawnTests
{
    [Fact]
    public void WhitePawn_OneStepForward_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (5, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_TwoStepsForwardOnFirstMove_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (4, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_BackwardMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (7, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_SidewaysMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (6, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_DiagonalNoCapture_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (5, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_TooFarMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (3, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_OneStepForward_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (2, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_TwoStepsForwardOnFirstMove_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (3, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_BackwardMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (0, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_SidewaysMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (1, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_DiagonalNoCapture_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (2, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_TooFarMove_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (4, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_DiagonalCapture_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        var enemy = new Pawn(PieceColour.Black);
        board.SetPiece((6, 0), pawn);
        board.SetPiece((5, 1), enemy);
        var move = new ChessMove((6, 0), (5, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void WhitePawn_DiagonalOwnPiece_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        var own = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        board.SetPiece((5, 1), own);
        var move = new ChessMove((6, 0), (5, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_DiagonalEmpty_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((6, 0), pawn);
        var move = new ChessMove((6, 0), (5, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_DiagonalCapture_IsValid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        var enemy = new Pawn(PieceColour.White);
        board.SetPiece((1, 0), pawn);
        board.SetPiece((2, 1), enemy);
        var move = new ChessMove((1, 0), (2, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeTrue();
    }

    [Fact]
    public void BlackPawn_DiagonalOwnPiece_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        var own = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        board.SetPiece((2, 1), own);
        var move = new ChessMove((1, 0), (2, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_DiagonalEmpty_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        board.SetPiece((1, 0), pawn);
        var move = new ChessMove((1, 0), (2, 1));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_ForwardBlocked_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        var blocker = new Pawn(PieceColour.Black);
        board.SetPiece((6, 0), pawn);
        board.SetPiece((5, 0), blocker);
        var move = new ChessMove((6, 0), (5, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_ForwardBlocked_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        var blocker = new Pawn(PieceColour.White);
        board.SetPiece((1, 0), pawn);
        board.SetPiece((2, 0), blocker);
        var move = new ChessMove((1, 0), (2, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void WhitePawn_TwoSquaresBlocked_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.White);
        var blocker = new Pawn(PieceColour.Black);
        board.SetPiece((6, 0), pawn);
        board.SetPiece((5, 0), blocker);
        var move = new ChessMove((6, 0), (4, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }

    [Fact]
    public void BlackPawn_TwoSquaresBlocked_IsInvalid()
    {
        var board = new ChessBoard();
        var pawn = new Pawn(PieceColour.Black);
        var blocker = new Pawn(PieceColour.White);
        board.SetPiece((1, 0), pawn);
        board.SetPiece((2, 0), blocker);
        var move = new ChessMove((1, 0), (3, 0));

        var attemptedMove = pawn.ValidMove(move, board);

        attemptedMove.Should().BeFalse();
    }
}