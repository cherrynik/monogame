using GameUi.App.State;

namespace GameUi.Features.PauseMenu;

public static class PauseHotkeyResolver
{
    public static UiAction? Resolve(PauseHotkeyState current, PauseHotkeyState previous)
    {
        if (current.Resume && !previous.Resume)
        {
            return UiAction.Resume;
        }

        if (current.OpenSettings && !previous.OpenSettings)
        {
            return UiAction.OpenSettings;
        }

        if (current.OpenRestart && !previous.OpenRestart)
        {
            return UiAction.OpenRestart;
        }

        if (current.Exit && !previous.Exit)
        {
            return UiAction.RequestExit;
        }

        return null;
    }
}
