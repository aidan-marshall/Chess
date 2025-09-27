using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class QueenTest
{
    private readonly IChessBoard _board;

    public QueenTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    public static IEnumerable<object[]> QueenValidMoves =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(3, 7) }, // right (rook-like)
            new object[] { new Position(3, 3), new Position(7, 3) }, // down (rook-like)
            new object[] { new Position(3, 3), new Position(0, 0) }, // up-left (bishop-like)
            new object[] { new Position(3, 3), new Position(6, 6) }  // down-right (bishop-like)
        };

    [Theory]
    [MemberData(nameof(QueenValidMoves))]
    public void ValidMove_ShouldSucceed_WhenMovingInAnyValidDirection(Position from, Position to)
    {
        var queen = new Queen(PieceColour.White);
        _board.SetPiece(from, queen);
        var move = new Move(from, to);

        bool isValid = queen.ValidMove(move, _board);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        var queen = new Queen(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);

        var from = new Position(3, 3);
        var to = new Position(7, 7);

        _board.SetPiece(from, queen);
        _board.SetPiece(to, enemyPawn);

        var move = new Move(from, to);

        queen.ValidMove(move, _board).Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedDiagonally()
    {
        var queen = new Queen(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black);

        var from = new Position(2, 2);
        var to = new Position(6, 6);
        var blocker = new Position(4, 4);

        _board.SetPiece(from, queen);
        _board.SetPiece(blocker, blockingPawn);

        var move = new Move(from, to);

        queen.ValidMove(move, _board).Should().BeFalse("the diagonal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedHorizontally()
    {
        var queen = new Queen(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.White);

        var from = new Position(3, 1);
        var to = new Position(3, 6);
        var blocker = new Position(3, 4);

        _board.SetPiece(from, queen);
        _board.SetPiece(blocker, blockingPawn);

        var move = new Move(from, to);

        queen.ValidMove(move, _board).Should().BeFalse("the horizontal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        var queen = new Queen(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);

        var from = new Position(3, 3);
        var to = new Position(3, 7);

        _board.SetPiece(from, queen);
        _board.SetPiece(to, friendlyPawn);

        var move = new Move(from, to);

        queen.ValidMove(move, _board).Should().BeFalse("a piece cannot capture a friendly piece");
    }

    public static IEnumerable<object[]> QueenInvalidMoves =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(1, 2) }, // invalid knight-like
            new object[] { new Position(3, 3), new Position(5, 4) }  // another invalid knight-like
        };

    [Theory]
    [MemberData(nameof(QueenInvalidMoves))]
    public void InvalidMove_ShouldFail_WhenMovePatternIsInvalid(Position from, Position to)
    {
        var queen = new Queen(PieceColour.White);
        _board.SetPiece(from, queen);

        var move = new Move(from, to);

        queen.ValidMove(move, _board).Should().BeFalse("a queen cannot move like a knight");
    }
}
