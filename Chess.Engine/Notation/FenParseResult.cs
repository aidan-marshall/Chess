using Chess.Engine.Pieces;

namespace Chess.Engine.Notation;

/// <summary>
/// Result of parsing a FEN string
/// </summary>
internal sealed record FenParseResult(
    bool IsSuccess,
    PieceColour ToMove = PieceColour.White,
    int HalfMoveClock = 0,
    int FullMoveNumber = 1,
    string? ErrorMessage = null)
{
    public static FenParseResult Success(PieceColour toMove, int halfMoveClock, int fullMoveNumber)
        => new(true, toMove, halfMoveClock, fullMoveNumber);

    public static FenParseResult Error(string message)
        => new(false, ErrorMessage: message);
}