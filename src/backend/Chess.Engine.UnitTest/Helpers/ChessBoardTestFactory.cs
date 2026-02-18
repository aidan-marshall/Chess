using Chess.Engine.Board;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine.UnitTest.Helpers;

/// <summary>
/// Factory class for creating specialized chess board configurations for testing.
/// Provides reusable board setups that avoid repetition across test files.
/// </summary>
public static class ChessBoardTestFactory
{
    /// <summary>
    /// Creates a board with a white pawn on e7 (row 1, column 4), ready for promotion.
    /// Includes minimal pieces to make the board valid (both kings).
    /// </summary>
    internal static IChessBoard CreateBoardWithWhitePawnOnE7()
    {
        var board = new ChessBoard();
        board.Clear();
        board.SetPiece(Position.Of(1, 4), Piece.Pawn(PieceColour.White));
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        board.SetPiece(Position.Of(0, 6), Piece.King(PieceColour.Black));
        return board;
    }

    /// <summary>
    /// Creates a board with a black pawn on e2 (row 6, column 4), ready for promotion.
    /// </summary>
    internal static IChessBoard CreateBoardWithBlackPawnOnE2()
    {
        var board = new ChessBoard();
        board.Clear();
        board.SetPiece(Position.Of(6, 4), Piece.Pawn(PieceColour.Black));
        board.SetPiece(Position.Of(0, 4), Piece.King(PieceColour.Black));
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        return board;
    }

    /// <summary>
    /// Creates a board where a white pawn on e7 can capture a black piece on d8.
    /// Tests promotion with capture mechanics.
    /// </summary>
    internal static IChessBoard CreateBoardWithPromotionCapture()
    {
        var board = new ChessBoard();
        board.Clear();
        board.SetPiece(Position.Of(1, 4), Piece.Pawn(PieceColour.White));     // White pawn on e7
        board.SetPiece(Position.Of(0, 3), Piece.Rook(PieceColour.Black));     // Black rook on d8
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        board.SetPiece(Position.Of(0, 4), Piece.King(PieceColour.Black));
        return board;
    }

    /// <summary>
    /// Creates a board where promoting to a queen results in check to the black king.
    /// </summary>
    internal static IChessBoard CreateBoardWithPromotionCheck()
    {
        var board = new ChessBoard();
        board.Clear();
        board.SetPiece(Position.Of(1, 4), Piece.Pawn(PieceColour.White));     // White pawn on e7
        board.SetPiece(Position.Of(0, 3), Piece.King(PieceColour.Black));     // Black king on e8 (directly in line)
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        return board;
    }

    /// <summary>
    /// Creates a board with multiple white pawns on different files (a and h) 
    /// near promotion rank, enabling testing of multiple promotion scenarios.
    /// </summary>
    internal static IChessBoard CreateBoardWithMultiplePawnsNearPromotion()
    {
        var board = new ChessBoard();
        board.Clear();
        board.SetPiece(Position.Of(1, 0), Piece.Pawn(PieceColour.White));     // a7
        board.SetPiece(Position.Of(1, 7), Piece.Pawn(PieceColour.White));     // h7
        board.SetPiece(Position.Of(7, 4), Piece.King(PieceColour.White));
        board.SetPiece(Position.Of(0, 4), Piece.King(PieceColour.Black));
        return board;
    }
}