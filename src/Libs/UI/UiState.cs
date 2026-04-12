namespace GameUi.App.State;

public readonly record struct UiState(
    bool IsPaused,
    UiDialogKind ActiveDialog,
    bool ShouldExit)
{
    public static UiState Default => new(false, UiDialogKind.None, false);
}
