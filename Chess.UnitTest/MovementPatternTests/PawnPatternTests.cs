using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class PawnPatternTests
{

    // White pawn movement

    [Fact]
    public void Pawn_White_OneStepForward_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
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
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
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
        Assert.False(result.Matches);
    }

    [Fact]
    public void Pawn_White_DiagonalCapture_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Capture, result.SpecialMoveType);
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
        Assert.False(result.Matches);
    }

    [Fact]
    public void Pawn_White_SingleStepIntoLastRank_IsPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(1, 4, 0, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Promotion, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_White_DiagonalCaptureIntoLastRank_IsPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(1, 4, 0, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Promotion, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_White_MoveNotIntoLastRank_IsNotPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.White);
        var move = Move.Of(6, 4, 5, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    // Black pawn movement

    [Fact]
    public void Pawn_Black_OneStepForward_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 2, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_Black_TwoStepForward_FromStart_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 3, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_Black_TwoStepForward_NotFromStart_IsInvalid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(2, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result.Matches);
    }

    [Fact]
    public void Pawn_Black_DiagonalCapture_IsValid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 3, 2, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Capture, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_Black_BackwardsMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 0, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result.Matches);
    }

    [Fact]
    public void Pawn_Black_SingleStepIntoLastRank_IsPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(6, 4, 7, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Promotion, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_Black_DiagonalCaptureIntoLastRank_IsPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(6, 3, 7, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.Promotion, result.SpecialMoveType);
    }

    [Fact]
    public void Pawn_Black_MoveNotIntoLastRank_IsNotPromotion()
    {
        // Arrange
        var piece = Piece.Pawn(PieceColour.Black);
        var move = Move.Of(1, 4, 2, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }
}
