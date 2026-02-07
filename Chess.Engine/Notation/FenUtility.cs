using Chess.Engine.Board;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using System.Text;

namespace Chess.Engine.Notation;

/// <summary>
/// Utility for converting chess positions to/from FEN (Forsyth-Edwards Notation)
/// FEN format: [piece placement] [active color] [castling] [en passant] [halfmove clock] [fullmove number]
/// Example: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
/// https://www.chess.com/terms/fen-chess#what-is-fen
/// </summary>
internal static class FenUtility
{
    private const string StartingPositionFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    /// <summary>
    /// Converts the current board state to FEN notation
    /// </summary>
    public static string ToFen(
        IChessBoard board,
        PieceColour toMove,
        int halfMoveClock,
        int fullMoveNumber)
    {
        var sb = new StringBuilder();

        // 1. Piece placement (from rank 0 to rank 7, which is 8th rank to 1st rank in chess)
        for (int row = 0; row < 8; row++)
        {
            var emptySquares = 0;

            for (int col = 0; col < 8; col++)
            {
                var piece = board.GetPiece(Position.Of(row, col));

                if (piece == null)
                {
                    emptySquares++;
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        sb.Append(emptySquares);
                        emptySquares = 0;
                    }
                    sb.Append(GetPieceChar(piece));
                }
            }

            if (emptySquares > 0)
                sb.Append(emptySquares);

            if (row < 7)
                sb.Append('/');
        }

        // 2. Active color
        sb.Append(' ');
        sb.Append(toMove == PieceColour.White ? 'w' : 'b');

        // 3. Castling availability
        sb.Append(' ');
        sb.Append(GetCastlingString(board.CastlingRights));

        // 4. En passant target square
        sb.Append(' ');
        sb.Append(GetEnPassantString(board.EnPassantTargetSquare));

        // 5. Halfmove clock (for 50-move rule)
        sb.Append(' ');
        sb.Append(halfMoveClock);

        // 6. Fullmove number (starts at 1, incremented after Black's move)
        sb.Append(' ');
        sb.Append(fullMoveNumber);

        return sb.ToString();
    }

    /// <summary>
    /// Loads a position from FEN notation into the board
    /// </summary>
    public static FenParseResult FromFen(string fen, IChessBoard board)
    {
        var parts = fen.Trim().Split(' ');

        if (parts.Length != 6)
            return FenParseResult.Error("Invalid FEN: must have 6 space-separated parts");

        try
        {
            // Clear the board first
            board.Clear();

            // 1. Parse piece placement
            if (!ParsePiecePlacement(parts[0], board))
                return FenParseResult.Error("Invalid piece placement in FEN");

            // 2. Parse active color
            var toMove = parts[1] switch
            {
                "w" => PieceColour.White,
                "b" => PieceColour.Black,
                _ => throw new FormatException("Invalid active color")
            };

            // 3. Parse castling rights
            var castlingRights = ParseCastlingRights(parts[2]);

            // 4. Parse en passant square
            var enPassantSquare = ParseEnPassantSquare(parts[3]);

            // 5. Parse halfmove clock
            if (!int.TryParse(parts[4], out int halfMoveClock) || halfMoveClock < 0)
                return FenParseResult.Error("Invalid halfmove clock");

            // 6. Parse fullmove number
            if (!int.TryParse(parts[5], out int fullMoveNumber) || fullMoveNumber < 1)
                return FenParseResult.Error("Invalid fullmove number");

            // Apply parsed values to board
            board.CastlingRights = castlingRights;
            board.EnPassantTargetSquare = enPassantSquare;

            return FenParseResult.Success(toMove, halfMoveClock, fullMoveNumber);
        }
        catch (Exception ex)
        {
            return FenParseResult.Error($"Error parsing FEN: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the FEN string for the standard starting position
    /// </summary>
    public static string GetStartingPositionFen() => StartingPositionFen;

    private static bool ParsePiecePlacement(string piecePlacement, IChessBoard board)
    {
        var ranks = piecePlacement.Split('/');

        if (ranks.Length != 8)
            return false;

        for (int row = 0; row < 8; row++)
        {
            var col = 0;
            foreach (char c in ranks[row])
            {
                if (char.IsDigit(c))
                {
                    // Empty squares
                    var emptyCount = c - '0';
                    if (emptyCount < 1 || emptyCount > 8)
                        return false;
                    
                    col += emptyCount;
                }
                else
                {
                    // Piece
                    var piece = GetPieceFromChar(c);
                    if (piece == null || col >= 8)
                        return false;

                    board.SetPiece(Position.Of(row, col), piece);
                    col++;
                }
            }

            if (col != 8)
                return false; // Row doesn't have exactly 8 squares
        }

        return true;
    }

    private static CastlingRights ParseCastlingRights(string castlingString)
    {
        if (castlingString == "-")
            return CastlingRights.None;

        var rights = CastlingRights.None;

        foreach (char c in castlingString)
        {
            rights |= c switch
            {
                'K' => CastlingRights.WhiteKingSide,
                'Q' => CastlingRights.WhiteQueenSide,
                'k' => CastlingRights.BlackKingSide,
                'q' => CastlingRights.BlackQueenSide,
                _ => throw new FormatException($"Invalid castling character: {c}")
            };
        }

        return rights;
    }

    private static Position? ParseEnPassantSquare(string enPassantString)
    {
        if (enPassantString == "-")
            return null;

        if (enPassantString.Length != 2)
            throw new FormatException("Invalid en passant square format");

        var file = enPassantString[0];
        var rank = enPassantString[1];

        if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
            throw new FormatException("Invalid en passant square coordinates");

        // Convert algebraic notation to array indices
        // a-h maps to columns 0-7
        // ranks 1-8 map to rows 7-0 (inverted)
        var col = file - 'a';
        var row = 8 - (rank - '0');

        return Position.Of(row, col);
    }

    private static char GetPieceChar(Piece piece)
    {
        var c = piece.Type switch
        {
            PieceType.Pawn => 'p',
            PieceType.Rook => 'r',
            PieceType.Knight => 'n',
            PieceType.Bishop => 'b',
            PieceType.Queen => 'q',
            PieceType.King => 'k',
            _ => throw new ArgumentException("Unknown piece type")
        };

        return piece.Colour == PieceColour.White ? char.ToUpper(c) : c;
    }

    private static Piece? GetPieceFromChar(char c)
    {
        var colour = char.IsUpper(c) ? PieceColour.White : PieceColour.Black;
        var pieceChar = char.ToLower(c);

        var pieceType = pieceChar switch
        {
            'p' => PieceType.Pawn,
            'r' => PieceType.Rook,
            'n' => PieceType.Knight,
            'b' => PieceType.Bishop,
            'q' => PieceType.Queen,
            'k' => PieceType.King,
            _ => (PieceType?)null
        };

        return pieceType.HasValue ? new Piece(pieceType.Value, colour) : null;
    }

    private static string GetCastlingString(CastlingRights rights)
    {
        if (rights == CastlingRights.None)
            return "-";

        var sb = new StringBuilder();

        if (rights.HasFlag(CastlingRights.WhiteKingSide))
            sb.Append('K');
        if (rights.HasFlag(CastlingRights.WhiteQueenSide))
            sb.Append('Q');
        if (rights.HasFlag(CastlingRights.BlackKingSide))
            sb.Append('k');
        if (rights.HasFlag(CastlingRights.BlackQueenSide))
            sb.Append('q');

        return sb.ToString();
    }

    private static string GetEnPassantString(Position? enPassantSquare)
    {
        if (!enPassantSquare.HasValue)
            return "-";

        var pos = enPassantSquare.Value;
        
        // Convert array indices to algebraic notation
        // columns 0-7 map to a-h
        // rows 7-0 map to ranks 1-8
        var file = (char)('a' + pos.Column);
        var rank = (char)('8' - pos.Row);

        return $"{file}{rank}";
    }
}
