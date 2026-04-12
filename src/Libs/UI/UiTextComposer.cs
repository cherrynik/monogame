using GameUi.App.State;

namespace GameUi.Shared.UI;

public static class UiTextComposer
{
    public static string ComposeHud(UiTheme theme) =>
        $"{theme.RuntimeModeText} {theme.GameVersionText}  ·  {theme.HudText}";

    public static string ComposePauseCard(UiTheme theme, UiState state)
    {
        string dialogHint = state.ActiveDialog switch
        {
            UiDialogKind.SettingsNotImplemented => "\nSettings are not yet available.",
            UiDialogKind.RestartNotImplemented => "\nRestart is not yet available.",
            _ => string.Empty
        };

        return $"{theme.PauseFooter}{dialogHint}";
    }
}
