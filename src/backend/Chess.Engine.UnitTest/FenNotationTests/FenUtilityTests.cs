using Chess.Engine.Board;
using Chess.Engine.Moves;
using Chess.Engine.Notation;
using Chess.Engine.Pieces;

namespace Chess.Engine.Tests.FenNotation;

public class FenUtilityTests
{
    [Fact]
    public void ToFen_StartingPosition_ReturnsCorrectFen()
    {
        // Arrange
        var board = new ChessBoard();
        var toMove = PieceColour.White;
        var halfMoveClock = 0;
        var fullMoveNumber = 1;

        // Act
        var fen = FenUtility.ToFen(board, toMove, halfMoveClock, fullMoveNumber);

        // Assert
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", fen);
    }

    [Fact]
    public void FromFen_StartingPosition_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(PieceColour.White, result.ToMove);
        Assert.Equal(0, result.HalfMoveClock);
        Assert.Equal(1, result.FullMoveNumber);
        Assert.Equal(CastlingRights.WhiteKingSide | CastlingRights.WhiteQueenSide | 
                     CastlingRights.BlackKingSide | CastlingRights.BlackQueenSide, 
                     board.CastlingRights);
        Assert.Null(board.EnPassantTargetSquare);
    }

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", "After e4")]
    [InlineData("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2", "After e4 e5")]
    [InlineData("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1", "Castling test position")]
    [InlineData("8/8/8/4k3/8/8/4P3/4K3 w - - 0 1", "King and pawn endgame")]
    [InlineData("rnbqkbnr/ppp1p1pp/8/3pPp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3", "En passant available")]
    public void FenRoundTrip_VariousPositions_ReturnsOriginalFen(string originalFen, string description)
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();

        // Act
        var parseResult = FenUtility.FromFen(originalFen, board);
        Assert.True(parseResult.IsSuccess, $"Failed to parse FEN for {description}");

        var exportedFen = FenUtility.ToFen(board, parseResult.ToMove, parseResult.HalfMoveClock, parseResult.FullMoveNumber);

        // Assert
        Assert.Equal(originalFen, exportedFen);
    }

    [Fact]
    public void FromFen_AfterE4_LoadsCorrectPiecePositions()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        
        // Check white pawn moved to e4 (row 4, col 4)
        var e4Piece = board.GetPiece(Position.Of(4, 4));
        Assert.NotNull(e4Piece);
        Assert.Equal(PieceType.Pawn, e4Piece.Type);
        Assert.Equal(PieceColour.White, e4Piece.Colour);

        // Check e2 is now empty (row 6, col 4)
        var e2Piece = board.GetPiece(Position.Of(6, 4));
        Assert.Null(e2Piece);

        // Check en passant square
        Assert.NotNull(board.EnPassantTargetSquare);
        Assert.Equal(Position.Of(5, 4), board.EnPassantTargetSquare.Value); // e3

        // Check it's black's turn
        Assert.Equal(PieceColour.Black, result.ToMove);
    }

    [Fact]
    public void FromFen_CastlingRightsPartial_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w Kq - 0 1";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(CastlingRights.WhiteKingSide | CastlingRights.BlackQueenSide, board.CastlingRights);
    }

    [Fact]
    public void FromFen_NoCastlingRights_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w - - 0 1";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(CastlingRights.None, board.CastlingRights);
    }

    [Fact]
    public void FromFen_EnPassantSquare_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/ppp1p1pp/8/3pPp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(board.EnPassantTargetSquare);
        Assert.Equal(Position.Of(2, 5), board.EnPassantTargetSquare.Value); // f6
    }

    [Fact]
    public void FromFen_HalfMoveClockAndFullMoveNumber_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 5 10";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.HalfMoveClock);
        Assert.Equal(10, result.FullMoveNumber);
    }

    [Theory]
    [InlineData("invalid", "Invalid FEN: must have 6 space-separated parts")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", "Invalid FEN: must have 6 space-separated parts")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1", "Invalid active color")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - -1 1", "Invalid halfmove clock")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0", "Invalid fullmove number")]
    public void FromFen_InvalidFen_ReturnsError(string invalidFen, string expectedErrorSubstring)
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();

        // Act
        var result = FenUtility.FromFen(invalidFen, board);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains(expectedErrorSubstring, result.ErrorMessage);
    }

    [Fact]
    public void FromFen_InvalidPiecePlacement_TooManyRanks_ReturnsError()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; // 9 ranks

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void FromFen_InvalidPiecePlacement_InvalidCharacter_ReturnsError()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPXPPP/RNBQKBNR w KQkq - 0 1"; // X is invalid

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void FromFen_EmptyBoard_LoadsCorrectly()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();
        var fen = "8/8/8/8/8/8/8/8 w - - 0 1";

        // Act
        var result = FenUtility.FromFen(fen, board);

        // Assert
        Assert.True(result.IsSuccess);
        
        // Check all squares are empty
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Assert.Null(board.GetPiece(Position.Of(r, c)));
            }
        }
    }

    [Fact]
    public void GetStartingPositionFen_ReturnsCorrectFen()
    {
        // Act
        var fen = FenUtility.GetStartingPositionFen();

        // Assert
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", fen);
    }

    [Fact]
    public void ToFen_ComplexPosition_GeneratesCorrectFen()
    {
        // Arrange
        var board = new ChessBoard();
        board.Clear();

        // Set up a specific position manually
        board.SetPiece(Position.Of(0, 4), Piece.King(PieceColour.Black));
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        board.SetPiece(Position.Of(4, 3), Piece.Queen(PieceColour.White));
        board.SetPiece(Position.Of(1, 0), Piece.Pawn(PieceColour.Black));
        
        board.CastlingRights = CastlingRights.None;
        board.EnPassantTargetSquare = null;

        // Act
        var fen = FenUtility.ToFen(board, PieceColour.Black, 10, 15);

        // Assert
        Assert.Equal("4k3/p7/8/8/3Q4/8/8/4K3 b - - 10 15", fen);
    }

    [Fact]
    public void ToFen_BlackToMove_GeneratesCorrectActiveColor()
    {
        // Arrange
        var board = new ChessBoard();

        // Act
        var fen = FenUtility.ToFen(board, PieceColour.Black, 0, 1);

        // Assert
        Assert.Contains(" b ", fen);
    }

    [Fact]
    public void ToFen_WithEnPassant_GeneratesCorrectSquare()
    {
        // Arrange
        var board = new ChessBoard();
        board.EnPassantTargetSquare = Position.Of(5, 4); // e3

        // Act
        var fen = FenUtility.ToFen(board, PieceColour.Black, 0, 1);

        // Assert
        Assert.Contains(" e3 ", fen);
    }
}