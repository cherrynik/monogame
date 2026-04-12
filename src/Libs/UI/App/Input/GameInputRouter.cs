using GameUi.App.State;
using GameUi.Features.PauseMenu;
using Microsoft.Xna.Framework.Input;
using Services.Input;

namespace GameUi.App.Input;

public sealed class GameInputRouter
{
    private readonly IScancodeKeyboardStateSource _scancodeSource;
    private KeyboardState _previousKeyboardState;
    private PauseHotkeyState _previousPauseHotkeys;

    public GameInputRouter(IScancodeKeyboardStateSource scancodeSource)
    {
        _scancodeSource = scancodeSource;
    }

    public GameInputFrame Capture()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        PauseHotkeyState currentPauseHotkeys =
            BuildPauseHotkeyState(keyboardState, ReadPauseHotkeysFromScancodes());

        bool togglePauseRequested = IsEdgePressed(Keys.Escape, keyboardState);
        UiAction? pauseAction = PauseHotkeyResolver.Resolve(currentPauseHotkeys, _previousPauseHotkeys);

        _previousKeyboardState = keyboardState;
        _previousPauseHotkeys = currentPauseHotkeys;

        return new GameInputFrame(togglePauseRequested, pauseAction);
    }

    private bool IsEdgePressed(Keys key, KeyboardState currentKeyboardState) =>
        currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);

    private static PauseHotkeyState BuildPauseHotkeyState(KeyboardState keyboardState, PauseHotkeyState physicalHotkeys)
    {
        bool altDown = keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt);
        bool enterAlone = keyboardState.IsKeyDown(Keys.Enter) && !altDown;

        return new PauseHotkeyState(
            Resume: physicalHotkeys.Resume || keyboardState.IsKeyDown(Keys.R) || enterAlone,
            OpenSettings: physicalHotkeys.OpenSettings || keyboardState.IsKeyDown(Keys.S),
            OpenRestart: physicalHotkeys.OpenRestart || keyboardState.IsKeyDown(Keys.T),
            Exit: physicalHotkeys.Exit || keyboardState.IsKeyDown(Keys.E));
    }

    private PauseHotkeyState ReadPauseHotkeysFromScancodes()
    {
        const int scanR = 21;
        const int scanS = 22;
        const int scanT = 23;
        const int scanE = 8;

        if (!_scancodeSource.TryGetState(out ScancodeKeyboardState state))
        {
            return default;
        }

        return new PauseHotkeyState(
            Resume: state.IsPressed(scanR),
            OpenSettings: state.IsPressed(scanS),
            OpenRestart: state.IsPressed(scanT),
            Exit: state.IsPressed(scanE));
    }
}
