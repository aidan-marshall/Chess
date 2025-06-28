using Chess.Core;
using Chess.Core.Pieces;

namespace Chess.UnitTest;

public class ChessBoardUnitTest
{
    [Fact]
    public void CreatingChessBoard_ShouldSetupBoardCorrectly()
    {
        // Arrange & Act
        var board = new ChessBoard();

        // Assert

        // 1. Check White's back rank (row 7)
        AssertPiece(board, (7, 0), typeof(Rook), PieceColour.White);
        AssertPiece(board, (7, 1), typeof(Knight), PieceColour.White);
        AssertPiece(board, (7, 2), typeof(Bishop), PieceColour.White);
        AssertPiece(board, (7, 3), typeof(Queen), PieceColour.White);
        AssertPiece(board, (7, 4), typeof(King), PieceColour.White);
        AssertPiece(board, (7, 5), typeof(Bishop), PieceColour.White);
        AssertPiece(board, (7, 6), typeof(Knight), PieceColour.White);
        AssertPiece(board, (7, 7), typeof(Rook), PieceColour.White);

        // 2. Check White's pawns (row 6)
        for (var col = 0; col < 8; col++)
        {
            AssertPiece(board, (6, col), typeof(Pawn), PieceColour.White);
        }

        // 3. Check the empty middle ranks (rows 2, 3, 4, 5)
        for (var row = 2; row <= 5; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                Assert.Null(board.GetPiece((row, col)));
            }
        }

        // 4. Check Black's pawns (row 1)
        for (var col = 0; col < 8; col++)
        {
            AssertPiece(board, (1, col), typeof(Pawn), PieceColour.Black);
        }

        // 5. Check Black's back rank (row 0)
        AssertPiece(board, (0, 0), typeof(Rook), PieceColour.Black);
        AssertPiece(board, (0, 1), typeof(Knight), PieceColour.Black);
        AssertPiece(board, (0, 2), typeof(Bishop), PieceColour.Black);
        AssertPiece(board, (0, 3), typeof(Queen), PieceColour.Black);
        AssertPiece(board, (0, 4), typeof(King), PieceColour.Black);
        AssertPiece(board, (0, 5), typeof(Bishop), PieceColour.Black);
        AssertPiece(board, (0, 6), typeof(Knight), PieceColour.Black);
        AssertPiece(board, (0, 7), typeof(Rook), PieceColour.Black);
    }

    [Fact]
    public void NewBoard_LastMove_ShouldBeNull()
    {
        // Arrange & Act
        var board = new ChessBoard();

        // Assert
        Assert.Null(board.LastMove);
    }

    [Fact]
    public void SetPiece_ShouldPlacePieceOnBoard()
    {
        // Arrange
        var board = new ChessBoard(); // Start with a standard board
        var position = (4, 4);
        var newPiece = new Queen(PieceColour.White);

        // Ensure the square is empty first
        Assert.Null(board.GetPiece(position));

        // Act
        board.SetPiece(position, newPiece);

        // Assert
        var pieceOnBoard = board.GetPiece(position);
        Assert.NotNull(pieceOnBoard);
        // Assert.Same checks for reference equality. We want to be sure it's the exact same object.
        Assert.Same(newPiece, pieceOnBoard);
    }

    /// <summary>
    /// Helper method to avoid repetitive assertion code.
    /// It checks if a piece at a given position is not null,
    /// of the expected type, and of the expected color.
    /// </summary>
    private void AssertPiece(IChessBoard board, (int row, int col) position, Type expectedType, PieceColour expectedColour)
    {
        var piece = board.GetPiece(position);
        Assert.NotNull(piece);
        Assert.IsType(expectedType, piece);
        Assert.Equal(expectedColour, piece.Colour);
    }
}