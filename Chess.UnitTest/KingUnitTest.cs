using Chess.Core;
using Chess.Core.Pieces;
using FluentAssertions;

namespace Chess.UnitTest;

public class KingUnitTest
{
    [Theory]
    [InlineData(3, 3, 2, 2)] // up-left
    [InlineData(3, 3, 2, 3)] // up
    [InlineData(3, 3, 2, 4)] // up-right
    [InlineData(3, 3, 3, 2)] // left
    [InlineData(3, 3, 3, 4)] // right
    [InlineData(3, 3, 4, 2)] // down-left
    [InlineData(3, 3, 4, 3)] // down
    [InlineData(3, 3, 4, 4)] // down-right
    public void King_CanMove_OneSquareInAnyDirection_AllDirections(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var board = new ChessBoard();
        var king = new King(PieceColour.White);
        board.SetPiece((fromRow, fromCol), king);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        var attemptedMove = king.ValidMove(move, board);

        // Assert
        attemptedMove.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 0, -1, -1, false)]
    [InlineData(0, 0, -1, 0, false)]
    [InlineData(0, 0, -1, 1, false)]
    [InlineData(0, 0, 0, -1, false)]
    [InlineData(0, 0, 0, 1, true)]
    [InlineData(0, 0, 1, -1, false)]
    [InlineData(0, 0, 1, 0, true)]
    [InlineData(0, 0, 1, 1, true)]
    public void King_CannotMove_OffBoard(int fromRow, int fromCol, int toRow, int toCol, bool expectedValid)
    {
        // Arrange
        var board = new ChessBoard();
        var king = new King(PieceColour.White);
        board.SetPiece((fromRow, fromCol), king);
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        var isValid = king.ValidMove(move, board);

        // Assert
        isValid.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData(4, 4, 3, 3, true)]
    [InlineData(4, 4, 3, 4, true)]
    [InlineData(4, 4, 3, 5, true)]
    [InlineData(4, 4, 4, 3, true)]
    [InlineData(4, 4, 4, 5, true)]
    [InlineData(4, 4, 5, 3, true)]
    [InlineData(4, 4, 5, 4, true)]
    [InlineData(4, 4, 5, 5, false)] // occupied by own piece
    public void King_CannotMove_IntoSquareOccupiedByOwnPiece(int fromRow, int fromCol, int toRow, int toCol, bool expectedValid)
    {
        // Arrange
        var board = new ChessBoard();
        var king = new King(PieceColour.White);
        var pawn = new Pawn(PieceColour.White);
        board.SetPiece((fromRow, fromCol), king);
        board.SetPiece((5, 5), pawn);

        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));

        // Act
        var isValid = king.ValidMove(move, board);

        // Assert
        isValid.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData(4, 4, 5, 5)]
    public void King_CanCapture_OpponentPiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        // Arrange
        var board = new ChessBoard();
        var king = new King(PieceColour.White);
        var enemyPawn = new Pawn(PieceColour.Black);
        board.SetPiece((fromRow, fromCol), king);
        board.SetPiece((toRow, toCol), enemyPawn);

        // Act
        var move = new ChessMove((fromRow, fromCol), (toRow, toCol));
        var canCapture = king.ValidMove(move, board);

        // Assert
        canCapture.Should().BeTrue();
    }
}
