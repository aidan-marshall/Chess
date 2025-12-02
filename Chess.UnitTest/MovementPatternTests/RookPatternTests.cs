using Chess.Core.Validation;

namespace Chess.Core.UnitTest.MovementPatternTests;

public class RookPatternTests
{
    [Fact]
    public void Rook_MoveUp_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 0, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_MoveDown_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 7, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_MoveLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 4, 0);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_MoveRight_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 4, 7);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_LongVerticalMove_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(1, 3, 6, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_LongHorizontalMove_IsValid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(2, 5, 2, 1);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Rook_DiagonalMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 6, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Rook_KnightLikeMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 5, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Rook_ZeroMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Rook(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }
}
