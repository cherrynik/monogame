using GameUi.App.State;
using NUnit.Framework;

namespace E2ETests.Services;

public class UiFlowE2ETests
{
    [Test]
    public void PauseFlow_ToggleToPaused_OpenSettings_DismissAndResume()
    {
        var store = new UiStore();

        store.Dispatch(UiAction.TogglePause);
        store.Dispatch(UiAction.OpenSettings);
        store.Dispatch(UiAction.DismissDialog);
        store.Dispatch(UiAction.Resume);

        Assert.That(store.State.IsPaused, Is.False);
        Assert.That(store.State.ActiveDialog, Is.EqualTo(UiDialogKind.None));
        Assert.That(store.State.ShouldExit, Is.False);
    }

    [Test]
    public void ExitFlow_RequestAndHandleExit_ClearsExitFlag()
    {
        var store = new UiStore();

        store.Dispatch(UiAction.TogglePause);
        store.Dispatch(UiAction.RequestExit);
        Assert.That(store.State.ShouldExit, Is.True);

        store.Dispatch(UiAction.ExitHandled);
        Assert.That(store.State.ShouldExit, Is.False);
    }
}
