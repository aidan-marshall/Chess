using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class RookTest
{
    private readonly ChessBoard _board;

    public RookTest()
    {
        _board = new ChessBoard();
        _board.Clear();
    }

    // --- I. Tests for VALID Moves ---

    [Theory]
    [InlineData(3, 3, 3, 7)] // Move right
    [InlineData(3, 3, 3, 0)] // Move left
    public void ValidMove_ShouldSucceed_WhenMovingHorizontallyOnEmptyBoard(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), rook);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(3, 3, 0, 3)] // Move up
    [InlineData(3, 3, 7, 3)] // Move down
    public void ValidMove_ShouldSucceed_WhenMovingVerticallyOnEmptyBoard(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), rook);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidMove_ShouldSucceed_WhenCapturingEnemyPiece()
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((3, 3), rook);
        _board.SetPiece((3, 7), enemyPawn);
        var move = new ChessMove((3, 3), (3, 7));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeTrue();
    }

    // --- II. Tests for INVALID Moves ---

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedHorizontally()
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.Black);
        _board.SetPiece((3, 3), rook);
        _board.SetPiece((3, 5), blockingPawn); // Blocker is between start and end
        var move = new ChessMove((3, 3), (3, 7));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the horizontal path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenPathIsBlockedVertically()
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        var blockingPawn = new Pawn(PieceColour.White); // Blocker can be friendly
        _board.SetPiece((3, 3), rook);
        _board.SetPiece((5, 3), blockingPawn); // Blocker is between start and end
        var move = new ChessMove((3, 3), (7, 3));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("the vertical path is blocked");
    }

    [Fact]
    public void InvalidMove_ShouldFail_WhenLandingOnFriendlyPiece()
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        var friendlyPawn = new Pawn(PieceColour.White);
        _board.SetPiece((3, 3), rook);
        _board.SetPiece((3, 7), friendlyPawn);
        var move = new ChessMove((3, 3), (3, 7));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse("a piece cannot capture a friendly piece");
    }

    [Theory]
    [InlineData(3, 3, 5, 5)] // Invalid Diagonal
    [InlineData(3, 3, 4, 5)] // Invalid Knight-move
    public void InvalidMove_ShouldFail_WhenMovePatternIsNotStraight(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var rook = new Rook(PieceColour.White);
        _board.SetPiece((fromRow, fromCol), rook);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        bool isValid = rook.ValidMove(move, _board);

        // Assert
        isValid.Should().BeFalse();
    }
}