using GameUi.App.State;
using GameUi.Shared.UI;

namespace UnitTests.Services;

public class UiTextComposerTests
{
    [Test]
    public void ComposeHud_ContainsRuntimeAndMovementInfo()
    {
        UiTheme theme = UiTheme.Default;

        string hud = UiTextComposer.ComposeHud(theme);

        Assert.That(hud, Does.Contain(theme.HudText));
        Assert.That(hud, Does.Contain(theme.RuntimeModeText));
        Assert.That(hud, Does.Contain(theme.GameVersionText));
    }

    [Test]
    public void ComposePauseCard_ContainsDialogHint_WhenSettingsDialogActive()
    {
        UiTheme theme = UiTheme.Default;
        UiState state = UiState.Default with
        {
            IsPaused = true,
            ActiveDialog = UiDialogKind.SettingsNotImplemented
        };

        string card = UiTextComposer.ComposePauseCard(theme, state);

        Assert.That(card, Does.Contain(theme.PauseFooter));
        Assert.That(card, Does.Contain("Settings are not yet available."));
    }
}
