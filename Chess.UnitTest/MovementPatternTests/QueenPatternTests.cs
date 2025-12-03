using Chess.Engine;
using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class QueenPatternTests
{
    [Fact]
    public void Queen_OneStepUp_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 3, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepDown_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 5, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 4, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepRight_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 4, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepDiagonalUpLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 3, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepDiagonalUpRight_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 3, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepDiagonalDownLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 5, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_OneStepDiagonalDownRight_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 5, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_LongVerticalMove_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 0, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_LongHorizontalMove_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 4, 7);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_LongDiagonalMove_IsValid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 7, 7);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Queen_InvalidKnightMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 6, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Queen_InvalidRandomMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 6, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Queen_ZeroMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Queen(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }
}
