using GameUi.App.State;

namespace GameUi.App.Input;

public readonly record struct GameInputFrame(
    bool TogglePauseRequested,
    UiAction? PauseMenuAction);
