namespace GameUi.Features.PauseMenu;

public readonly record struct PauseHotkeyState(
    bool Resume,
    bool OpenSettings,
    bool OpenRestart,
    bool Exit);
