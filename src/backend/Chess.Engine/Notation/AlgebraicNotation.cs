using Chess.Engine.Board;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using System.Text;

namespace Chess.Engine.Notation;

/// <summary>
/// Utility for converting moves to Standard Algebraic Notation (SAN).
/// Examples: e4, Nf3, Bxe5, O-O, e8=Q+, Qh4#
/// </summary>
internal static class AlgebraicNotation
{
    /// <summary>
    /// Converts a move to Standard Algebraic Notation (SAN).
    /// </summary>
    /// <param name="move">The move to convert</param>
    /// <param name="board">The board state BEFORE the move (needed for disambiguation)</param>
    /// <param name="movedPiece">The piece being moved</param>
    /// <param name="isCapture">Whether the move captures a piece</param>
    /// <param name="isCheck">Whether the move results in check</param>
    /// <param name="isCheckmate">Whether the move results in checkmate</param>
    /// <param name="promotionPieceType">The piece type if this is a promotion (null otherwise)</param>
    /// <param name="isCastleKingSide">Whether this is kingside castling</param>
    /// <param name="isCastleQueenSide">Whether this is queenside castling</param>
    /// <returns>The move in algebraic notation</returns>
    public static string ToAlgebraic(
        Move move,
        IChessBoard board,
        Piece movedPiece,
        bool isCapture,
        bool isCheck,
        bool isCheckmate,
        PieceType? promotionPieceType = null,
        bool isCastleKingSide = false,
        bool isCastleQueenSide = false)
    {
        var notation = new StringBuilder();

        // Handle castling
        if (isCastleKingSide)
        {
            notation.Append("O-O");
        }
        else if (isCastleQueenSide)
        {
            notation.Append("O-O-O");
        }
        else
        {
            // Piece identifier (not for pawns unless capturing)
            if (movedPiece.Type != PieceType.Pawn)
            {
                notation.Append(GetPieceSymbol(movedPiece.Type));

                // Add disambiguation if needed (e.g., Nbd7 if two knights can move to d7)
                var disambiguation = GetDisambiguation(move, board, movedPiece);
                notation.Append(disambiguation);
            }
            else if (isCapture)
            {
                // For pawn captures, include the starting file
                notation.Append(GetFile(move.From.Column));
            }

            // Capture notation
            if (isCapture)
            {
                notation.Append('x');
            }

            // Destination square
            notation.Append(GetSquareName(move.To));

            // Promotion
            if (promotionPieceType.HasValue)
            {
                notation.Append('=');
                notation.Append(GetPieceSymbol(promotionPieceType.Value));
            }
        }

        // Check/Checkmate notation
        if (isCheckmate)
        {
            notation.Append('#');
        }
        else if (isCheck)
        {
            notation.Append('+');
        }

        return notation.ToString();
    }

    /// <summary>
    /// Gets the disambiguation string for a move (e.g., "d" in "Nbd7").
    /// Needed when multiple pieces of the same type can move to the same square.
    /// </summary>
    private static string GetDisambiguation(Move move, IChessBoard board, Piece movedPiece)
    {
        // Find all pieces of the same type and color that could move to the destination
        var ambiguousPieces = new List<Position>();

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var pos = Position.Of(r, c);

                // Skip the piece that's actually moving
                if (pos.Equals(move.From))
                    continue;

                var piece = board.GetPiece(pos);

                // Check if this is the same type and color
                if (piece?.Type == movedPiece.Type && piece.Colour == movedPiece.Colour)
                {
                    ambiguousPieces.Add(pos);
                }
            }
        }

        if (ambiguousPieces.Count == 0)
        {
            return ""; // No ambiguity
        }

        // Check if file (column) disambiguation is enough
        bool fileUnique = ambiguousPieces.All(p => p.Column != move.From.Column);
        if (fileUnique)
        {
            return GetFile(move.From.Column).ToString();
        }

        // Check if rank (row) disambiguation is enough
        bool rankUnique = ambiguousPieces.All(p => p.Row != move.From.Row);
        if (rankUnique)
        {
            return GetRank(move.From.Row).ToString();
        }

        // Need both file and rank
        return $"{GetFile(move.From.Column)}{GetRank(move.From.Row)}";
    }

    /// <summary>
    /// Gets the piece symbol for algebraic notation (K, Q, R, B, N, P).
    /// </summary>
    private static char GetPieceSymbol(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.King => 'K',
            PieceType.Queen => 'Q',
            PieceType.Rook => 'R',
            PieceType.Bishop => 'B',
            PieceType.Knight => 'N',
            PieceType.Pawn => 'P',
            _ => throw new ArgumentException($"Unknown piece type: {pieceType}")
        };
    }

    /// <summary>
    /// Converts a position to square name (e.g., "e4", "a1").
    /// </summary>
    private static string GetSquareName(Position position)
    {
        char file = GetFile(position.Column);
        char rank = GetRank(position.Row);
        return $"{file}{rank}";
    }

    /// <summary>
    /// Converts column index to file letter (0='a', 7='h').
    /// </summary>
    private static char GetFile(int column)
    {
        return (char)('a' + column);
    }

    /// <summary>
    /// Converts row index to rank number (0='8', 7='1').
    /// </summary>
    private static char GetRank(int row)
    {
        return (char)('8' - row);
    }
}
