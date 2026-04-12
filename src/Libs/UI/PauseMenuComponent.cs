using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using GameUi.App.State;
using GameUi.Entities.PauseMenu;
using GameUi.Shared.UI;

namespace GameUi.Widgets.PauseMenu;

public sealed class PauseMenuComponent
{
    private static readonly Color Overlay   = new(16,  12,   8, 160);
    private static readonly Color Stone     = new(200, 192, 176);
    private static readonly Color StoneEdge = new(164, 156, 140);
    private static readonly Color Outline   = new(40,   32,  24);
    private static readonly Color BtnHover  = new(216, 208, 192);
    private static readonly Color BtnPress  = new(180, 172, 156);
    private static readonly Color TitleGold = new(224, 164,  48);
    private static readonly Color TextDark  = new(48,   36,  24);
    private static readonly Color TextMuted = new(120, 108,  88);

    private readonly Panel _overlay;
    private readonly Label _footer;
    private readonly UiTheme _theme;

    public PauseMenuComponent(UiTheme theme, UiVisualStyle style, Action<UiAction> onAction)
    {
        _theme = theme;

        _overlay = new Panel
        {
            Background = new SolidBrush(Overlay),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var stack = new VerticalStackPanel
        {
            Spacing = 4,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        stack.Widgets.Add(new Label
        {
            Text = theme.PauseTitle.ToUpperInvariant(),
            TextColor = TitleGold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 4)
        });

        IReadOnlyList<PauseMenuItem> items = PauseMenuModel.Create(theme);
        for (int i = 0; i < items.Count; i++)
        {
            PauseMenuItem item = items[i];

            var btn = new Button
            {
                Width = style.ButtonWidth,
                Height = 32,
                Content = new Label
                {
                    Text = item.Title.ToUpperInvariant(),
                    TextColor = TextDark,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidBrush(Stone),
                OverBackground = new SolidBrush(BtnHover),
                PressedBackground = new SolidBrush(BtnPress),
                Border = new SolidBrush(Outline),
                BorderThickness = new Thickness(3),
                Padding = new Thickness(0)
            };
            btn.Click += (_, _) => onAction(item.Action);
            stack.Widgets.Add(btn);
        }

        _footer = new Label
        {
            TextColor = TextMuted,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 4, 0, 0)
        };
        stack.Widgets.Add(_footer);

        _overlay.Widgets.Add(stack);
    }

    public Widget Widget => _overlay;

    public void Render(UiState state)
    {
        _overlay.Visible = state.IsPaused;
        _footer.Text = UiTextComposer.ComposePauseCard(_theme, state);
    }
}
