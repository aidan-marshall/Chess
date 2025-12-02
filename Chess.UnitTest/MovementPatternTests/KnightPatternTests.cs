using Chess.Core.Validation;

namespace Chess.Core.UnitTest.MovementPatternTests;

public class KnightPatternTests
{
    [Fact]
    public void Knight_Move_TwoUpOneRight_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 2, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoUpOneLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 2, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoDownOneRight_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoDownOneLeft_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 3);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoRightOneUp_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 3, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoRightOneDown_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoLeftOneUp_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 3, 2);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_TwoLeftOneDown_IsValid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 2);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Knight_Move_Straight_IsInvalid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 4, 7);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Knight_Move_Diagonal_IsInvalid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 6, 6);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Knight_ZeroMove_IsInvalid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 4, 4);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Knight_Move_ThreeUpOneRight_IsInvalid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 1, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Knight_Move_LikeKing_IsInvalid()
    {
        // Arrange
        var piece = Piece.Knight(PieceColour.White);
        var move = Move.Of(4, 4, 5, 5);

        // Act
        var result = MovementPatternValidator.MatchesMovementPattern(piece, move);

        // Assert
        Assert.False(result);
    }
}
