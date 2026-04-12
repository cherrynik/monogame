using GameUi.App.State;
using GameUi.Features.PauseMenu;

namespace UnitTests.Services;

public class PauseHotkeyResolverTests
{
    [Test]
    public void Resolve_ReturnsResume_OnResumeEdge()
    {
        var previous = new PauseHotkeyState(Resume: false, OpenSettings: false, OpenRestart: false, Exit: false);
        var current = new PauseHotkeyState(Resume: true, OpenSettings: false, OpenRestart: false, Exit: false);

        UiAction? action = PauseHotkeyResolver.Resolve(current, previous);

        Assert.That(action, Is.EqualTo(UiAction.Resume));
    }

    [Test]
    public void Resolve_ReturnsOpenSettings_OnSettingsEdge()
    {
        var previous = default(PauseHotkeyState);
        var current = new PauseHotkeyState(Resume: false, OpenSettings: true, OpenRestart: false, Exit: false);

        UiAction? action = PauseHotkeyResolver.Resolve(current, previous);

        Assert.That(action, Is.EqualTo(UiAction.OpenSettings));
    }

    [Test]
    public void Resolve_ReturnsNull_WhenNoEdge()
    {
        var previous = new PauseHotkeyState(Resume: true, OpenSettings: false, OpenRestart: false, Exit: false);
        var current = new PauseHotkeyState(Resume: true, OpenSettings: false, OpenRestart: false, Exit: false);

        UiAction? action = PauseHotkeyResolver.Resolve(current, previous);

        Assert.That(action, Is.Null);
    }
}
