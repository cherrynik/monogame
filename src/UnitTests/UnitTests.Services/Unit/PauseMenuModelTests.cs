using GameUi.App.State;
using GameUi.Entities.PauseMenu;
using GameUi.Shared.UI;

namespace UnitTests.Services;

public class PauseMenuModelTests
{
    [Test]
    public void Create_ReturnsExpectedActionOrder()
    {
        IReadOnlyList<PauseMenuItem> items = PauseMenuModel.Create(UiTheme.Default);

        Assert.That(items.Select(x => x.Action).ToArray(), Is.EqualTo(new[]
        {
            UiAction.Resume,
            UiAction.OpenSettings,
            UiAction.OpenRestart,
            UiAction.RequestExit
        }));
    }
}
