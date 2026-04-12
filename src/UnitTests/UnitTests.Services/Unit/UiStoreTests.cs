using GameUi.App.State;

namespace UnitTests.Services;

public class UiStoreTests
{
    [Test]
    public void Dispatch_UpdatesStoreState()
    {
        var store = new UiStore();

        UiState state = store.Dispatch(UiAction.TogglePause);

        Assert.That(state.IsPaused, Is.True);
        Assert.That(store.State.IsPaused, Is.True);
    }

    [Test]
    public void Dispatch_AppliesReducerTransitionsInOrder()
    {
        var store = new UiStore();

        store.Dispatch(UiAction.TogglePause);
        store.Dispatch(UiAction.OpenSettings);
        store.Dispatch(UiAction.DismissDialog);
        store.Dispatch(UiAction.Resume);

        Assert.That(store.State.IsPaused, Is.False);
        Assert.That(store.State.ActiveDialog, Is.EqualTo(UiDialogKind.None));
    }
}
