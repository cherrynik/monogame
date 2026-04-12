using System.Numerics;
using Components;
using Components.Data;
using Components.Events.Movement;
using Components.Tags;
using Scellecs.Morpeh;
using Services;
using Services.Movement;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public class MovementSystem : ISystem
{
    private readonly IMovement _movement;
    private Filter _movableFilter = default!;
    private Filter _movedEventsFilter = default!;
    public World World { get; set; }

    public MovementSystem(World world, IMovement movement)
    {
        World = world;
        _movement = movement;
    }

    public void OnAwake()
    {
        _movableFilter = World.Filter
            .With<InputMovableComponent>()
            .With<TransformComponent>()
            .Build();
        _movedEventsFilter = World.Filter
            .With<TransformMovedEvent>()
            .Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var transformStash = World.GetStash<TransformComponent>();
        var movedEventStash = World.GetStash<TransformMovedEvent>();

        foreach (Entity entity in _movedEventsFilter)
        {
            movedEventStash.Remove(entity);
        }

        foreach (Entity entity in _movableFilter)
        {
            ref TransformComponent transform = ref transformStash.Get(entity);
            Vector2 previousPosition = transform.Position;
            Vector2 nextPosition = _movement.Move(from: previousPosition, by: transform.Velocity);

            if (nextPosition.Equals(previousPosition))
            {
                continue;
            }

            transform.Position = nextPosition;
            if (!movedEventStash.Has(entity))
            {
                movedEventStash.Add(entity);
            }
        }
    }

    public void Dispose()
    {
    }
}
