using GameUi.App.State;
using GameUi.Shared.UI;

namespace GameUi.Entities.PauseMenu;

public static class PauseMenuModel
{
    public static IReadOnlyList<PauseMenuItem> Create(UiTheme theme) =>
    [
        new PauseMenuItem(theme.ResumeTitle, UiAction.Resume, "R  Enter"),
        new PauseMenuItem(theme.SettingsTitle, UiAction.OpenSettings, "S"),
        new PauseMenuItem(theme.RestartTitle, UiAction.OpenRestart, "T"),
        new PauseMenuItem(theme.ExitTitle, UiAction.RequestExit, "E")
    ];
}
