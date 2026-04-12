using GameUi.App.State;

namespace GameUi.App.Input;

public readonly record struct GameInputFrame(
    bool TogglePauseRequested,
    bool ToggleFullscreenRequested,
    UiAction? PauseMenuAction);
