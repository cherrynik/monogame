namespace GameUi.App.State;

public static class UiReducer
{
    public static UiState Reduce(UiState state, UiAction action) =>
        action switch
        {
            UiAction.TogglePause => state with
            {
                IsPaused = !state.IsPaused,
                ActiveDialog = UiDialogKind.None
            },
            UiAction.Resume => state with
            {
                IsPaused = false,
                ActiveDialog = UiDialogKind.None
            },
            UiAction.OpenSettings => state with { ActiveDialog = UiDialogKind.SettingsNotImplemented },
            UiAction.OpenRestart => state with { ActiveDialog = UiDialogKind.RestartNotImplemented },
            UiAction.DismissDialog => state with { ActiveDialog = UiDialogKind.None },
            UiAction.RequestExit => state with { ShouldExit = true },
            UiAction.ExitHandled => state with { ShouldExit = false },
            _ => state
        };
}
