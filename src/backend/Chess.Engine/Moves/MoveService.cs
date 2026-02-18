using Chess.Engine.Board;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.Moves;

internal class MoveService
{
    public static (Piece MovedPiece, Piece? CapturedPiece) ExecuteValidatedMove(
        IChessBoard board,
        Move move,
        MoveValidationResult moveValidationResult)
    {
        var piece = board.GetPiece(move.From) ?? throw new InvalidOperationException("No piece at the source position.");

        var capturedPiece = board.GetPiece(move.To);

        var enPassantCapture = HandleSpecialMove(moveValidationResult, board);

        if (enPassantCapture != null)
            capturedPiece = enPassantCapture;

        board.SetPiece(move.To, piece);
        board.SetPiece(move.From, null);

        piece.MoveAmount++;
        board.Moves.Add(move);

        return (piece, capturedPiece);
    }

    private static Piece? HandleSpecialMove(MoveValidationResult moveValidationResult, IChessBoard board)
    {
        return moveValidationResult.SpecialMoveType switch
        {
            SpecialMoveType.CastleKingSide or SpecialMoveType.CastleQueenSide => HandleCastling(moveValidationResult, board),
            SpecialMoveType.EnPassant => HandleEnPassant(moveValidationResult, board),
            _ => null,
        };
    }

    private static Piece? HandleEnPassant(MoveValidationResult moveValidationResult, IChessBoard board)
    {
        if (!moveValidationResult.EnPassantCapturedPawn.HasValue)
            throw new InvalidOperationException("En Passant move validation result missing captured pawn position.");

        var capturedPiece = board.GetPiece(moveValidationResult.EnPassantCapturedPawn.Value);
        board.SetPiece(moveValidationResult.EnPassantCapturedPawn.Value, null);

        return capturedPiece;
    }

    private static Piece? HandleCastling(MoveValidationResult moveValidationResult, IChessBoard board)
    {
        if (!moveValidationResult.CastlingRookMove.HasValue)
            throw new InvalidOperationException("Castling move validation result missing rook positions.");

        var rookMove = moveValidationResult.CastlingRookMove.Value;
        var rookPiece = board.GetPiece(rookMove.From) ?? throw new InvalidOperationException("No rook at the castling source position.");

        board.SetPiece(rookMove.To, rookPiece);
        board.SetPiece(rookMove.From, null);
        rookPiece.MoveAmount++;

        return null;
    }
}
