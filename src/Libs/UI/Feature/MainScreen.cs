using UI.Blocks;
using UI.Factories;

namespace UI.Feature;

public class MainScreen
{
    public MainScreen(UIFactory uiFactory)
    {
        uiFactory
            .AddTriggerCounter()
            .AddGameVersion();
    }
}
