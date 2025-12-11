using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.UnitTest.MovementPatternTests;

public class BishopPatternTests
{
    [Fact]
    public void Bishop_DiagonalUpRight_IsValid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 2, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Bishop_DiagonalUpLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 2, 2);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Bishop_DiagonalDownRight_IsValid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 6, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Bishop_DiagonalDownLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 6, 2);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.Normal, result);
    }

    [Fact]
    public void Bishop_HorizontalMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 4, 7);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Bishop_VerticalMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 7, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Bishop_NoMovement_IsInvalid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.None, result);
    }

    [Fact]
    public void Bishop_NonDiagonalMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Bishop(PieceColour.White);
        var move = Move.Of(4, 4, 5, 6); // Not a diagonal

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.Equal(MovementPatternType.None, result);
    }
}
