using System.Numerics;
using Components;
using Components.Data;
using Components.Tags;
using Scellecs.Morpeh;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public class MovementSystem : ISystem
{
    public World World { get; set; }


    public void OnAwake()
    {
        Console.WriteLine((nameof(MovementSystem), "OnAwake"));
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

            // Might be moved up too, but too simple for now
            if (transform.Velocity.Equals(Vector2.Zero))
            {
                continue;
            }

            transform.Position += Vector2.Normalize(transform.Velocity);

            // Console.WriteLine((nameof(MovementSystem), "OnUpdate", deltaTime, transform.Position));
        }
    }

    public void Dispose()
    {
    }
}
