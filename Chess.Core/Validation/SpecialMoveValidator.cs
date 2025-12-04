//namespace Chess.Engine.Validation;

//internal class SpecialMoveValidator
//{
//    public static (bool IsLegal, SpecialMoveType SpecialMoveType) Validate(Move move, IChessBoard board, Piece movingPiece, Piece? movingToPiece)
//    {
//        switch (movingPiece.Type)
//        {
//            case PieceType.Pawn:
//                // En Passant

//                // Promotion

//                break;
//            case PieceType.King:
//                // Castling
//                if (move.ColDiff == 2 && move.RowDiff == 0)
//                {
//                    return SpecialMoveType.CastleKingSide;
//                }
//                else if (move.ColDiff == -2 && move.RowDiff == 0)
//                {
//                    return SpecialMoveType.CastleQueenSide;
//                }
//                break;
//        }
//    }

//    private static (bool IsLegal, SpecialMoveType SpecialMoveType) EnPassant(Move move, IChessBoard board, Piece movingPiece, Piece? movingToPiece)
//    {
//        var direction = movingPiece.Colour.Direction();

//        // Is this an EnPassant pattern?
//        if (!(Math.Abs(move.ColDiff) == 1 && move.RowDiff == direction))
//        {
//            return (true, SpecialMoveType.None);
//        }

//        // Is this EnPassant move valid ?

//        // Is the target square empty ?
//        if (movingToPiece is not null)
//        {
//            return (false, SpecialMoveType.Promotion);
//        }






        



//        // Implement En Passant validation logic here
//        return (true, SpecialMoveType.EnPassant);
//    }

//    private static (bool IsLegal, SpecialMoveType SpecialMoveType) Promotion(Move move, IChessBoard board, Piece movingPiece)
//    {
//        if (!PromotionPattern(move, movingPiece))
//        {
//            return (true, SpecialMoveType.None);
//        }

        

//        // Implement Promotion validation logic here
//        return (true, SpecialMoveType.Promotion);
//    }

//    private static bool PromotionPattern(Move move, Piece movingPiece)
//    {
//        var targetRow = movingPiece.Colour == PieceColour.White ? 7 : 0;
//        return move.To.Row == targetRow && move.ColDiff == 0 && move.RowDiff == movingPiece.Colour.Direction();
//    }

//    private static (bool IsLegal, SpecialMoveType SpecialMoveType) Castling(Move move, IChessBoard board, Piece movingPiece)
//    {
//        // Implement Castling validation logic here
//        return (true, move.ColDiff == 2 ? SpecialMoveType.CastleKingSide : SpecialMoveType.CastleQueenSide);
//    }
//}
