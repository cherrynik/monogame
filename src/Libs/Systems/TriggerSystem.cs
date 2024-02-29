using Scellecs.Morpeh;

namespace Systems;

public class TriggerSystem : IFixedSystem
{
    public World World { get; set; }
    private bool _entered;

    public TriggerSystem(World world, CollisionSystem collisionSystem)
    {
        World = world;
        collisionSystem.RaiseTriggerEntered += HandleTriggerEntered;
    }

    private void HandleTriggerEntered(Entity sender, CustomEventArgs e)
    {
        if (_entered) return;

        Console.WriteLine(e.Message);
        _entered = true;
    }

    public void Dispose()
    {
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
    }
}
