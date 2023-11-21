using Myra.Graphics2D.UI;

namespace GameDesktop.Factories;

public class UIFactory
{
    private readonly Grid _grid;
    private readonly Label _label;
    private readonly ComboBox _comboBox;
    private readonly Button _button;
    private readonly SpinButton _spinButton;

    public UIFactory(Grid grid, Label label, ComboBox comboBox, Button button, SpinButton spinButton)
    {
        // Stylesheet.Current.ButtonStyle = new ButtonStyle
        // {
        //     Background = new ColoredRegion(new TextureRegion(pixel, new Rectangle(0, 0, 15, 15)), Color.Gold),
        //     Padding = new Thickness(5, 5),
        // };

        _grid = grid;
        _label = label;
        _comboBox = comboBox;
        _button = button;
        _spinButton = spinButton;
    }

    public void Build()
    {
        BuildComboBox();
        BuildButton();
        BuildSpinButton();

        Initialize();
    }

    private void BuildComboBox()
    {
        Grid.SetColumn(_comboBox, 1);
        Grid.SetRow(_comboBox, 0);
    }

    private void BuildButton()
    {
        Grid.SetColumn(_button, 0);
        Grid.SetRow(_button, 1);
    }

    private void BuildSpinButton()
    {
        Grid.SetColumn(_spinButton, 1);
        Grid.SetRow(_spinButton, 1);
    }

    private void Initialize()
    {
        _grid.Widgets.Add(_label);
        _grid.Widgets.Add(_comboBox);
        _grid.Widgets.Add(_button);
        _grid.Widgets.Add(_spinButton);
    }
}
