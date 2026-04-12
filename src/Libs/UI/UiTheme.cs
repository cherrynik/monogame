namespace GameUi.Shared.UI;

public sealed class UiTheme
{
    public string HudText { get; init; } = "WASD / Arrows to move";
    public string HudHint { get; init; } = "Esc: Pause  ·  Enter: Resume  ·  E: Exit  ·  Alt+Enter: Fullscreen";
    public string RuntimeModeText { get; init; } = "DEV";
    public string GameVersionText { get; init; } = "v0.0.0";
    public string PauseTitle { get; init; } = "Game Paused";
    public string ResumeTitle { get; init; } = "Resume";
    public string SettingsTitle { get; init; } = "Settings";
    public string RestartTitle { get; init; } = "Restart";
    public string ExitTitle { get; init; } = "Exit";
    public string PauseFooter { get; init; } = "Esc to close menu";

    public static UiTheme Default => new();
}
