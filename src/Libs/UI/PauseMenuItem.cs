using GameUi.App.State;

namespace GameUi.Entities.PauseMenu;

public readonly record struct PauseMenuItem(string Title, UiAction Action, string HotkeyHint);
