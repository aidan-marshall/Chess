using Chess.Engine.Board;
using Chess.Engine.Helpers;
using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine.Validation;

internal class SpecialMoveValidator
{

    public static MoveValidationResult Validate(
        MovementPatternType movementPattern,
        IChessBoard board,
        Piece movingPiece,
        Piece? targetPiece,
        Move move)
    {
        return movementPattern switch
        {
            MovementPatternType.Normal => MoveValidationResult.LegalNormal(),
            MovementPatternType.PawnCapture => PawnCapture(movingPiece, targetPiece, board, move),
            MovementPatternType.PawnDoubleStep => PawnDoubleStep(movingPiece, targetPiece),
            MovementPatternType.PawnPromotion => PawnPromotion(movingPiece, targetPiece, move),
            MovementPatternType.CastleKingSide => Castle(true, movingPiece, board),
            MovementPatternType.CastleQueenSide => Castle(false, movingPiece, board),
            _ => MoveValidationResult.Illegal()
        };
    }

    private static MoveValidationResult PawnCapture(
        Piece movingPiece,
        Piece? targetPiece,
        IChessBoard board,
        Move move)
    {
        // Handle Enpassant scenario
        if (targetPiece == null)
        {
            return EnPassant(
                movingPiece,
                board,
                move);
        }
        
        if (targetPiece.Colour == movingPiece.Colour)
            return MoveValidationResult.Illegal();

        return MoveValidationResult.LegalNormal();
    }

    private static MoveValidationResult PawnDoubleStep(Piece movingPiece, Piece? targetPiece)
    {
        var isLegal = targetPiece is null && !movingPiece.HasMoved;

        return isLegal ? MoveValidationResult.LegalNormal() : MoveValidationResult.Illegal();
    }

    private static MoveValidationResult PawnPromotion(
        Piece movingPiece,
        Piece? targetPiece,
        Move move)
    {
        var direction = movingPiece.Colour.Direction();

        // Must be a single-step forward OR capture forward
        var isSingleStep = move.ColDiff == 0 && move.RowDiff == direction;
        var isCapture = Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction;

        if (!isSingleStep && !isCapture)
            return MoveValidationResult.Illegal();

        // Single-step promotion must land on empty square
        if (isSingleStep && targetPiece != null)
            return MoveValidationResult.Illegal();

        // Capture promotion must capture enemy piece
        if (isCapture && (targetPiece == null || targetPiece.Colour == movingPiece.Colour))
            return MoveValidationResult.Illegal();

        return MoveValidationResult.LegalPromotion();
    }

    private static MoveValidationResult Castle(bool isKingSide, Piece king, IChessBoard board)
    {
        if (king.Type != PieceType.King)
            return MoveValidationResult.Illegal();

        if (king.HasMoved)
            return MoveValidationResult.Illegal();

        var positionOfRook = isKingSide
            ? Position.Of(king.Colour.HomeRank(), 7)
            : Position.Of(king.Colour.HomeRank(), 0);

        var rook = board.GetPiece(positionOfRook);

        if (rook is null ||
            rook.Type is not PieceType.Rook ||
            rook.Colour != king.Colour ||
            rook.HasMoved)
            return MoveValidationResult.Illegal();

        var rookTargetColumn = isKingSide ? 5 : 3;

        var rookTargetMove = Move.Of(
            from: positionOfRook,
            to: Position.Of(king.Colour.HomeRank(), rookTargetColumn));

        return MoveValidationResult.LegalCastling(rookTargetMove, isKingSide);
    }

    private static MoveValidationResult EnPassant(
        Piece movingPiece,
        IChessBoard board,
        Move move)
    {
        if (!board.EnPassantTargetSquare.HasValue)
            return MoveValidationResult.Illegal();

        var direction = movingPiece.Colour.Direction();
        var enpassantPiecePosition = board.EnPassantTargetSquare.Value;

        // Is this an EnPassant pattern?
        if (!(Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction))
            return MoveValidationResult.Illegal();

        // Is this EnPassant move valid ?
        if (move.To != enpassantPiecePosition)
            return MoveValidationResult.Illegal();

        var capturedPawnPosition = Position.Of(enpassantPiecePosition.Row - direction, enpassantPiecePosition.Column);

        var enpassantPiece = board.GetPiece(capturedPawnPosition);

        if (enpassantPiece is null ||
            enpassantPiece.Type != PieceType.Pawn ||
            enpassantPiece.Colour == movingPiece.Colour)
            return MoveValidationResult.Illegal();

        return MoveValidationResult.LegalEnPassant(capturedPawnPosition);
    }
}
