using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using GameUi.App.State;
using GameUi.Shared.UI;
using GameUi.Widgets.PauseMenu;

namespace GameUi.Pages.Game;

public sealed class GameUiView
{
    private static readonly Color Shadow  = new(0, 0, 0, 180);
    private static readonly Color TextMain = new(255, 255, 255, 220);
    private static readonly Color TextDim  = new(200, 200, 180, 120);

    private readonly Panel _hudPanel;
    private readonly PauseMenuComponent _pauseMenu;

    public GameUiView(Grid grid, UiTheme theme, UiVisualStyle style, Action<UiAction> onAction)
    {
        string versionLine = $"{theme.RuntimeModeText} {theme.GameVersionText}";
        string hintLine = theme.HudText;

        _hudPanel = new Panel
        {
            Margin = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        // Shadow layer (offset 2px right + 2px down)
        var shadowStack = new VerticalStackPanel
        {
            Spacing = 1,
            Margin = new Thickness(2, 2, 0, 0),
        };
        shadowStack.Widgets.Add(new Label { Text = versionLine, TextColor = Shadow });
        shadowStack.Widgets.Add(new Label { Text = hintLine, TextColor = Shadow });

        // Main text layer
        var textStack = new VerticalStackPanel { Spacing = 1 };
        textStack.Widgets.Add(new Label { Text = versionLine, TextColor = TextMain });
        textStack.Widgets.Add(new Label { Text = hintLine, TextColor = TextDim });

        _hudPanel.Widgets.Add(shadowStack);
        _hudPanel.Widgets.Add(textStack);

        _pauseMenu = new PauseMenuComponent(theme, style, onAction);

        Grid.SetColumn(_hudPanel, 0);
        Grid.SetRow(_hudPanel, 0);
        Grid.SetColumn(_pauseMenu.Widget, 0);
        Grid.SetRow(_pauseMenu.Widget, 0);

        grid.Widgets.Add(_hudPanel);
        grid.Widgets.Add(_pauseMenu.Widget);
    }

    public void Render(UiState state)
    {
        _hudPanel.Visible = !state.IsPaused;
        _pauseMenu.Render(state);
    }
}
