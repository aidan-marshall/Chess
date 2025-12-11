namespace Chess.Engine.Moves;

internal class MoveService
{
    public static void ExecuteMove(
        IChessBoard board,
        Move move)
    {
        var piece = board.GetPiece(move.From) ?? throw new InvalidOperationException("No piece at the source position.");

        board.SetPiece(move.To, piece);
        board.SetPiece(move.From, null);

        piece.MoveAmount++;

        // TODO: Handle special moves / handles captures etc.
    }
}
