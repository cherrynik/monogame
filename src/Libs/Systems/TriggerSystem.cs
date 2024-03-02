using Scellecs.Morpeh;

namespace Systems;

public class TriggerSystem : IFixedSystem
{
    public World World { get; set; }

    public TriggerSystem(World world, CollisionSystem collisionSystem)
    {
        World = world;
        collisionSystem.Entered += OnTriggerEntered;
        collisionSystem.Stay += OnTriggerStay;
        collisionSystem.Exited += OnTriggerExited;
    }

    private void OnTriggerEntered(Entity sender, Entity with, bool isTriggerEvent)
    {
    }

    private void OnTriggerStay(Entity sender, Entity with, bool isTriggerEvent)
    {
    }

    private void OnTriggerExited(Entity sender, Entity with)
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
