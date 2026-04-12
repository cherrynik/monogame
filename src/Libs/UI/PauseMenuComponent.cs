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
    private static readonly Color Shadow    = new(0, 0, 0, 180);

    private static readonly Color TitleGold = new(224, 176, 56);
    private static readonly Color SepGold   = new(160, 120, 40, 140);

    private static readonly Color BtnBg     = new(186, 174, 154);
    private static readonly Color BtnBorder = new(48, 36, 24);
    private static readonly Color BtnHover  = new(210, 198, 178);
    private static readonly Color BtnPress  = new(160, 148, 128);
    private static readonly Color BtnText   = new(48, 36, 24);

    private static readonly Color FooterClr = new(180, 170, 150, 160);

    private readonly Panel _root;
    private readonly Label _footer;
    private readonly Label _footerShadow;
    private readonly UiTheme _theme;

    public PauseMenuComponent(UiTheme theme, UiVisualStyle style, Action<UiAction> onAction)
    {
        _theme = theme;

        _root = new Panel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        var stack = new VerticalStackPanel
        {
            Spacing = style.VerticalSpacing,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        // Title with drop shadow
        stack.Widgets.Add(ShadowText(theme.PauseTitle.ToUpperInvariant(), TitleGold));

        // Gold separator
        stack.Widgets.Add(new Panel
        {
            Height = 2,
            Width = style.ButtonWidth - 20,
            Background = new SolidBrush(SepGold),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 2, 0, 6),
        });

        // Buttons
        IReadOnlyList<PauseMenuItem> items = PauseMenuModel.Create(theme);
        for (int i = 0; i < items.Count; i++)
        {
            PauseMenuItem item = items[i];
            char hotkey = item.HotkeyHint[0];

            var btn = new Button
            {
                Width = style.ButtonWidth,
                Height = 34,
                Content = new Label
                {
                    Text = $"[{hotkey}]  {item.Title.ToUpperInvariant()}",
                    TextColor = BtnText,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidBrush(BtnBg),
                OverBackground = new SolidBrush(BtnHover),
                PressedBackground = new SolidBrush(BtnPress),
                Border = new SolidBrush(BtnBorder),
                BorderThickness = new Thickness(3),
                Padding = new Thickness(0),
            };
            btn.Click += (_, _) => onAction(item.Action);
            stack.Widgets.Add(btn);
        }

        // Footer with drop shadow
        _footerShadow = new Label
        {
            TextColor = Shadow,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(2, 10, 0, 0),
        };
        _footer = new Label
        {
            TextColor = FooterClr,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 8, 0, 0),
        };

        var footerPanel = new Panel();
        footerPanel.Widgets.Add(_footerShadow);
        footerPanel.Widgets.Add(_footer);
        stack.Widgets.Add(footerPanel);

        _root.Widgets.Add(stack);
    }

    public Widget Widget => _root;

    public void Render(UiState state)
    {
        _root.Visible = state.IsPaused;
        string text = UiTextComposer.ComposePauseCard(_theme, state);
        _footer.Text = text;
        _footerShadow.Text = text;
    }

    private static Panel ShadowText(string text, Color color)
    {
        var panel = new Panel { HorizontalAlignment = HorizontalAlignment.Center };
        panel.Widgets.Add(new Label
        {
            Text = text,
            TextColor = Shadow,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(2, 2, 0, 0),
        });
        panel.Widgets.Add(new Label
        {
            Text = text,
            TextColor = color,
            HorizontalAlignment = HorizontalAlignment.Center,
        });
        return panel;
    }
}
