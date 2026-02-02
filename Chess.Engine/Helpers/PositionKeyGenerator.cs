using Chess.Engine.Board;
using Chess.Engine.Pieces;
using Chess.Engine.Moves;
using System.Text;

namespace Chess.Engine.Helpers;

internal static class PositionKeyGenerator
{
    public static string GeneratePositionKey(IChessBoard board, PieceColour toMove)
    {
        var sb = new StringBuilder();

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var position = Position.Of(r, c);
                var piece = board.GetPiece(position);

                sb.Append(piece is null
                    ? '.'
                    : PieceChar(piece));
            }
        }

        sb.Append('|');
        sb.Append(toMove == PieceColour.White ? 'w' : 'b');
        sb.Append('|');
        sb.Append(CastlingRightsString(board.CastlingRights));
        sb.Append('|');
        sb.Append(board.EnPassantTargetSquare?.ToString() ?? "-");

        return sb.ToString();
    }

    private static char PieceChar(Piece piece)
    {
        var c = piece.Type switch
        {
            PieceType.Pawn => 'p',
            PieceType.Rook => 'r',
            PieceType.Knight => 'n',
            PieceType.Bishop => 'b',
            PieceType.Queen => 'q',
            PieceType.King => 'k',
            _ => throw new ApplicationException("Unknown piece type")
        };

        return piece.Colour == PieceColour.White ? char.ToUpper(c) : c;
    }

    private static string CastlingRightsString(CastlingRights rights)
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
}
