using Components.Data;
using Components.Tags;
using Scellecs.Morpeh;
using Services.Implementations.Movement;

namespace Systems;

public class MovementSystem(World world, IMovement movement) : IFixedSystem
{
    public World World { get; set; } = world;
    private const float SpeedMultiplier = 10f;

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
            ref MovableComponent movableComponent = ref e.GetComponent<MovableComponent>();

            transform.Position = movement.Move(from: transform.Position,
                by: transform.Velocity,
                speed: SpeedMultiplier * deltaTime * movableComponent.Speed);
        }
    }

    public void Dispose()
    {
    }
}
