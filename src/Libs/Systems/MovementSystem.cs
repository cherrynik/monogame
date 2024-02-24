using System.Numerics;
using Components;
using Components.Data;
using Components.Tags;
using Scellecs.Morpeh;
using Services;
using Services.Movement;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public class MovementSystem : ISystem
{
    private readonly IMovement _movement;
    public World World { get; set; }

    public MovementSystem(World world, IMovement movement)
    {
        World = world;
        _movement = movement;
    }

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

            transform.Position = _movement.Move(from: transform.Position, by: transform.Velocity);
        }
    }

    public void Dispose()
    {
    }
}
