using GameUi.App.State;

namespace UnitTests.Services;

public class UiReducerTests
{
    [Test]
    public void TogglePause_TogglesPauseFlag()
    {
        UiState state = UiState.Default;

        UiState paused = UiReducer.Reduce(state, UiAction.TogglePause);
        UiState resumed = UiReducer.Reduce(paused, UiAction.TogglePause);

        Assert.That(paused.IsPaused, Is.True);
        Assert.That(resumed.IsPaused, Is.False);
    }

    [Test]
    public void OpenSettings_SetsSettingsDialogState()
    {
        UiState state = UiState.Default with { IsPaused = true };

        UiState next = UiReducer.Reduce(state, UiAction.OpenSettings);

        Assert.That(next.ActiveDialog, Is.EqualTo(UiDialogKind.SettingsNotImplemented));
    }

    [Test]
    public void RequestExitAndExitHandled_FlipsExitFlag()
    {
        UiState state = UiState.Default with { IsPaused = true };

        UiState exitRequested = UiReducer.Reduce(state, UiAction.RequestExit);
        UiState exitHandled = UiReducer.Reduce(exitRequested, UiAction.ExitHandled);

        Assert.That(exitRequested.ShouldExit, Is.True);
        Assert.That(exitHandled.ShouldExit, Is.False);
    }
}
