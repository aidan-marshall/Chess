using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class KingTest
{
    private readonly IChessBoard _board;

    public KingTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    public static IEnumerable<object[]> KingMoves_Valid =>
        new List<object[]>
        {
            new object[] { new Position(3, 3), new Position(2, 2) }, // Up-Left
            new object[] { new Position(3, 3), new Position(2, 3) }, // Up
            new object[] { new Position(3, 3), new Position(2, 4) }, // Up-Right
            new object[] { new Position(3, 3), new Position(3, 2) }, // Left
            new object[] { new Position(3, 3), new Position(3, 4) }, // Right
            new object[] { new Position(3, 3), new Position(4, 2) }, // Down-Left
            new object[] { new Position(3, 3), new Position(4, 3) }, // Down
            new object[] { new Position(3, 3), new Position(4, 4) }  // Down-Right
        };

    [Theory]
    [MemberData(nameof(KingMoves_Valid))]
    public void ValidMove_ShouldSucceed_WhenMovingOneSquareToEmpty(Position from, Position to)
    {
        var king = new King(PieceColour.White);
        _board.SetPiece(from, king);
        var move = new Move(from, to);

        bool isValid = king.ValidMove(move, _board);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        var king = new King(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        var from = new Position(3, 3);
        var to = new Position(4, 4);

        _board.SetPiece(from, king);
        _board.SetPiece(to, enemyPawn);
        var move = new Move(from, to);

        bool isValid = king.ValidMove(move, _board);

        isValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> KingMoves_Castle =>
        new List<object[]>
        {
            new object[] { new Position(7, 4), new Position(7, 6) }, // White King-side
            new object[] { new Position(7, 4), new Position(7, 2) }, // White Queen-side
            new object[] { new Position(0, 4), new Position(0, 6) }, // Black King-side
            new object[] { new Position(0, 4), new Position(0, 2) }  // Black Queen-side
        };

    [Theory]
    [MemberData(nameof(KingMoves_Castle))]
    public void ValidMove_ShouldSucceed_WhenIdentifyingCastlePattern(Position from, Position to)
    {
        var kingColour = (from.Row == 7) ? PieceColour.White : PieceColour.Black;
        var king = new King(kingColour);

        _board.SetPiece(from, king);
        var move = new Move(from, to);

        bool isValid = king.ValidMove(move, _board);

        isValid.Should().BeTrue("the king should recognize a two-square horizontal move as a castling attempt");
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        var king = new King(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        var from = new Position(3, 3);
        var to = new Position(4, 4);

        _board.SetPiece(from, king);
        _board.SetPiece(to, friendlyPawn);
        var move = new Move(from, to);

        bool isValid = king.ValidMove(move, _board);

        isValid.Should().BeFalse("a king cannot capture a friendly piece");
    }

    public static IEnumerable<object[]> KingMoves_Invalid =>
        [
            [new Position(3, 3), new Position(5, 5)], // Two squares diagonally
            [new Position(3, 3), new Position(1, 4)]  // Knight-like
        ];

    [Theory]
    [MemberData(nameof(KingMoves_Invalid))]
    public void InvalidMove_ShouldFail_WhenMovePatternIsInvalid(Position from, Position to)
    {
        var king = new King(PieceColour.White);
        _board.SetPiece(from, king);
        var move = new Move(from, to);

        bool isValid = king.ValidMove(move, _board);

        isValid.Should().BeFalse();
    }
}
