using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using GameUi.App.State;
using GameUi.Shared.UI;
using GameUi.Widgets.PauseMenu;

namespace GameUi.Pages.Game;

public sealed class GameUiView
{
    private static readonly Color HudBg   = new(24, 36, 52, 210);
    private static readonly Color HudText = new(180, 192, 212);

    private readonly Label _hudLabel;
    private readonly PauseMenuComponent _pauseMenu;
    private readonly UiTheme _theme;

    public GameUiView(Grid grid, UiTheme theme, UiVisualStyle style, Action<UiAction> onAction)
    {
        _theme = theme;

        _hudLabel = new Label
        {
            TextColor = HudText,
            Padding = new Thickness(6, 3),
            Margin = new Thickness(4),
            Background = new SolidBrush(HudBg),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        _pauseMenu = new PauseMenuComponent(theme, style, onAction);

        Grid.SetColumn(_hudLabel, 0);
        Grid.SetRow(_hudLabel, 0);
        Grid.SetColumn(_pauseMenu.Widget, 0);
        Grid.SetRow(_pauseMenu.Widget, 0);

        grid.Widgets.Add(_hudLabel);
        grid.Widgets.Add(_pauseMenu.Widget);
    }

    public void Render(UiState state)
    {
        _hudLabel.Visible = !state.IsPaused;
        _hudLabel.Text = UiTextComposer.ComposeHud(_theme);
        _pauseMenu.Render(state);
    }
}
