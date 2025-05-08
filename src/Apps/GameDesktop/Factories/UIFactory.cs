using Myra.Graphics2D.UI;

namespace GameDesktop.Factories;

public class UIFactory(Grid grid, Label label, ComboBox comboBox, Button button, SpinButton spinButton)
{
    // Stylesheet.Current.ButtonStyle = new ButtonStyle
    // {
    //     Background = new ColoredRegion(new TextureRegion(pixel, new Rectangle(0, 0, 15, 15)), Color.Gold),
    //     Padding = new Thickness(5, 5),
    // };

    public void Build()
    {
        BuildComboBox();
        BuildButton();
        BuildSpinButton();

        Initialize();
    }

    private void BuildComboBox()
    {
        Grid.SetColumn(comboBox, 1);
        Grid.SetRow(comboBox, 0);
    }

    private void BuildButton()
    {
        Grid.SetColumn(button, 0);
        Grid.SetRow(button, 1);
    }

    private void BuildSpinButton()
    {
        Grid.SetColumn(spinButton, 1);
        Grid.SetRow(spinButton, 1);
    }

    private void Initialize()
    {
        grid.Widgets.Add(label);
        grid.Widgets.Add(comboBox);
        grid.Widgets.Add(button);
        grid.Widgets.Add(spinButton);
    }
}
