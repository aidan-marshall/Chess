using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class KnightPatternTests
{
    [Fact]
    public void Knight_Move_TwoUpOneRight_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 2, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoUpOneLeft_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 2, 3);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoDownOneRight_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoDownOneLeft_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 3);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoRightOneUp_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 3, 6);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoRightOneDown_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 6);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoLeftOneUp_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 3, 2);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Knight_Move_TwoLeftOneDown_IsValid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 2);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    // --- INVALID MOVES ---

    [Fact]
    public void Knight_Move_Straight_IsInvalid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 4, 7);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Knight_Move_Diagonal_IsInvalid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 6);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Knight_ZeroMove_IsInvalid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Knight_Move_ThreeUpOneRight_IsInvalid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 1, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Knight_Move_LikeKing_IsInvalid()
    {
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }
}
