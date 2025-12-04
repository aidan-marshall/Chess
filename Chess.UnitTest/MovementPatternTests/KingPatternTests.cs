using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class KingPatternTests
{
    [Fact]
    public void King_OneStepUp_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepDown_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepLeft_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepRight_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepDiagonalUpLeft_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepDiagonalUpRight_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 3, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepDiagonalDownLeft_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_OneStepDiagonalDownRight_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 5, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.None, result.SpecialMoveType);
    }

    [Fact]
    public void King_CastleRight_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(7, 4, 7, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.CastleKingSide, result.SpecialMoveType);
    }

    [Fact]
    public void King_CastleLeft_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(7, 4, 7, 2);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
        Assert.Equal(SpecialMoveType.CastleQueenSide, result.SpecialMoveType);
    }

    [Fact]
    public void King_TwoSquaresUp_IsInvalid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 2, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result.Matches);
    }

    [Fact]
    public void King_TwoSquaresRight_IsValid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result.Matches);
    }

    [Fact]
    public void King_ZeroMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result.Matches);
    }

    [Fact]
    public void King_KnightLikeMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.King(PieceColour.White);
        var move = Move.Of(4, 4, 6, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result.Matches);
    }

}
