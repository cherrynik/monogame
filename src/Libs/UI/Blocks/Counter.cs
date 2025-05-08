using Myra.Graphics2D.UI;

namespace UI.Blocks;

// TODO: try yaml with myra?
public class Counter(Label counterLabel)
{
    public int Count;
    private readonly Label _counterLabel = counterLabel;

    public Counter(Container container, Label counterLabel) : this(counterLabel) =>
        container.Widgets.Add(_counterLabel);

    public void UpdateText() => _counterLabel.Text = $"Count: {Count}";

    // Test methods
    public void SayEntered() => _counterLabel.Text = "Entered";

    public void SayExited() => _counterLabel.Text = "Exited";
}
