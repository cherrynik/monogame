using Scellecs.Morpeh;

namespace Systems;

public class TriggerSystem : IFixedSystem
{
    public World World { get; set; }
    // TODO: List/Dictionary of intersecting colliders, so we can know who entered, staying, exited

    public TriggerSystem(World world, CollisionSystem collisionSystem)
    {
        World = world;
        collisionSystem.RaiseTriggerIntersect += OnTriggerIntersect;
    }

    private void OnTriggerIntersect(Entity sender, CustomEventArgs e)
    {
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
