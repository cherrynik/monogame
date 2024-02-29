using Myra.Graphics2D.UI;
using Scellecs.Morpeh;
using Systems;

namespace UI;

// TODO: Use factory method, reactive update, divide on more UI components, use DI
public class TestElement
{
    public readonly Panel Panel;
    public readonly Label Counter;
    public readonly Label Version;
    private int _counter;

    public TestElement(Panel panel, Label counter, Label version, CollisionSystem collisionSystem)
    {
        Panel = panel;
        Counter = counter;
        Version = version;

        collisionSystem.RaiseTriggerEntered += HandleTriggerEntered;
    }

    public TestElement AddCounterLabel()
    {
        Panel.Widgets.Add(Counter);

        UpdateCounterText();

        return this;
    }

    public TestElement AddVersionLabel()
    {
        Panel.Widgets.Add(Version);
        return this;
    }

    private void HandleTriggerEntered(Entity sender, CustomEventArgs args)
    {
        ++_counter;
        UpdateCounterText();
    }

    private void UpdateCounterText() => Counter.Text = $"Counter: {_counter}";
    // public TestElement()
    // {
    // Panel.Widgets.Add(Label);
    // }
}
