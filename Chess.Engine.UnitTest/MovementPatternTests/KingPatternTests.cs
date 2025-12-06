using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class KingPatternTests
{
    [Fact]
    public void King_OneStepUp_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepDown_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepLeft_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 3);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepRight_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepDiagonalUpLeft_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 3);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepDiagonalUpRight_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepDiagonalDownLeft_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 3);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void King_OneStepDiagonalDownRight_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    // --- Castling ---

    [Fact]
    public void King_CastleRight_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(7, 4, 7, 6);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.CastleKingSide, result);
    }

    [Fact]
    public void King_CastleLeft_IsValid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(7, 4, 7, 2);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.CastleQueenSide, result);
    }

    // --- Invalid Moves ---

    [Fact]
    public void King_TwoSquaresUp_IsInvalid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 2, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void King_TwoSquaresRight_IsInvalid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 6);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void King_ZeroMove_IsInvalid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void King_KnightLikeMove_IsInvalid()
    {
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 6, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }
}
