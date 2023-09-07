using System.Numerics;

namespace Components;

public class Class1
{
    public void Main()
    {
        var c = Contexts.sharedInstance;
        var e = c.game.CreateEntity();
        e.AddTransform(new());
    }
}
