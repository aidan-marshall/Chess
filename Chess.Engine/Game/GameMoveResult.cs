using Chess.Engine.Moves;
using Chess.Engine.Pieces;
using Chess.Engine.Validation;

namespace Chess.Engine.Game;

public sealed record GameMoveResult(
    bool IsSuccess,
    MoveValidationResult? ValidationResult = null,
    Piece? CapturedPiece = null,
    GameState? NewGameState = null,
    string? Error = null,
    DrawReason? DrawReason = null,
    PieceType? PromotedToPieceType = null)
{
    internal static GameMoveResult Illegal(string error)
        => new(
            false,
            Error: error
            );

    internal static GameMoveResult Legal(
        MoveValidationResult validation,
        Piece? captured,
        GameState newState)
        => new(
            true,
            validation,
            captured,
            newState
            );

    internal static GameMoveResult RequiresPromotion(MoveValidationResult moveValidationResult, Piece? capturedPiece)
    {
        return new(
            true,
            ValidationResult: moveValidationResult,
            CapturedPiece: capturedPiece,
            NewGameState: GameState.PromotionPending
            );
    }

    /// <summary>
    /// Creates a result for a successful promotion completion.
    /// </summary>
    internal static GameMoveResult PromotionCompleted(
        PieceType promotedTo,
        GameState newState,
        Piece? capturedPiece = null)
        => new(
            true,
            NewGameState: newState,
            CapturedPiece: capturedPiece,
            PromotedToPieceType: promotedTo
            );
}
