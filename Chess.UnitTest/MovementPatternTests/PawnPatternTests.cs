using Chess.Engine;
using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class PawnPatternTests
{
    [Fact]
    public void Pawn_White_OneStepForward_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Pawn_White_TwoStepForward_FromStart_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Pawn_White_TwoStepForward_NotFromStart_IsInvalid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(5, 4, 3, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Pawn_White_DiagonalCapturePattern_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Pawn_White_BackwardsMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 7, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }
}
