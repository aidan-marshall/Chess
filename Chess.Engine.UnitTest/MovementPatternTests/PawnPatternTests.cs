using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class PawnPatternTests
{
    //
    // White pawn
    //

    [Fact]
    public void Pawn_White_OneStepForward_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Pawn_White_TwoStepForward_FromStart_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 4, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnDoubleStep, result);
    }

    [Fact]
    public void Pawn_White_TwoStepForward_NotFromStart_IsInvalid()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(5, 4, 3, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Pawn_White_DiagonalCapture_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnCapture, result);
    }

    [Fact]
    public void Pawn_White_BackwardsMove_IsInvalid()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 7, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Pawn_White_SingleStepIntoLastRank_IsPromotion()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(1, 4, 0, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnPromotion, result);
    }

    [Fact]
    public void Pawn_White_DiagonalCaptureIntoLastRank_IsPromotion()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(1, 4, 0, 5);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnPromotion, result);
    }

    [Fact]
    public void Pawn_White_MoveNotIntoLastRank_IsNotPromotion()
    {
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    //
    // Black Pawn
    //

    [Fact]
    public void Pawn_Black_OneStepForward_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 2, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Pawn_Black_TwoStepForward_FromStart_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 3, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnDoubleStep, result);
    }

    [Fact]
    public void Pawn_Black_TwoStepForward_NotFromStart_IsInvalid()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(2, 4, 4, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Pawn_Black_DiagonalCapture_IsValid()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 3, 2, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnCapture, result);
    }

    [Fact]
    public void Pawn_Black_BackwardsMove_IsInvalid()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 0, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Pawn_Black_SingleStepIntoLastRank_IsPromotion()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(6, 4, 7, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnPromotion, result);
    }

    [Fact]
    public void Pawn_Black_DiagonalCaptureIntoLastRank_IsPromotion()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(6, 3, 7, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.PawnPromotion, result);
    }

    [Fact]
    public void Pawn_Black_MoveNotIntoLastRank_IsNotPromotion()
    {
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 2, 4);

        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        Assert.Equal(MovementPatternType.Normal, result);
    }
}
