using Components;
using Components.Data;
using Components.Tags;
using Scellecs.Morpeh;

namespace Systems;

public class InputSystem(World world, IInputScanner inputScanner) : IFixedSystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<InputMovableComponent>()
            .With<TransformComponent>()
            .Build();

        foreach (Entity e in filter)
        {
            ref TransformComponent transform = ref e.GetComponent<TransformComponent>();
            transform.Velocity = inputScanner.GetDirection();
        }
    }

    public void Dispose()
    {
    }
}
