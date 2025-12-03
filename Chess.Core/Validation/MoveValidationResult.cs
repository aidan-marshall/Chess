namespace Chess.Engine.Validation;

internal readonly record struct MoveValidationResult(bool IsValid, string? ErrorMessage)
{
    public bool IsValid { get; init; } = IsValid;
    public string? ErrorMessage { get; init; } = ErrorMessage;

    public static MoveValidationResult Valid()
        => new()
        {
            IsValid = true,
            ErrorMessage = null
        };

    public static MoveValidationResult Invalid(string errorMessage)
        => new()
        {
            IsValid = false,
            ErrorMessage = errorMessage
        };
}
