﻿using Components.Data;
using Microsoft.Xna.Framework;
using Scellecs.Morpeh;
using Vector2 = System.Numerics.Vector2;

namespace Systems;

public class CustomEventArgs(string message) : EventArgs
{
    public string Message = message;
}

// Input & Collision systems both have to be fixed execute systems,
// otherwise it'll lead to the desynchronized behaviour.
public class CollisionSystem(World world) : IFixedSystem
{
    public World World { get; set; } = world;

    public delegate void TriggerHandler(Entity sender, CustomEventArgs args);
    public event TriggerHandler? RaiseTriggerEntered;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<RectangleColliderComponent>()
            .With<TransformComponent>()
            .Build();

        foreach (Entity e in filter)
        {
            ref var leftTransform = ref e.GetComponent<TransformComponent>();
            ref var leftCollider = ref e.GetComponent<RectangleColliderComponent>();

            foreach (Entity other in filter)
            {
                if (e.Equals(other)) continue;

                ref var rightTransform = ref other.GetComponent<TransformComponent>();
                ref var rightCollider = ref other.GetComponent<RectangleColliderComponent>();

                if (!AreColliding(new(leftTransform, leftCollider),
                        new(rightTransform, rightCollider))) continue;

                if (leftCollider.IsTrigger || rightCollider.IsTrigger)
                    OnRaiseTriggerEnter(other, new CustomEventArgs("Entered"));
                else HandleCollision(ref leftTransform, ref rightTransform);
            }
        }
    }

    private void OnRaiseTriggerEnter(Entity sender, CustomEventArgs customEventArgs)
    {
        var raiseEvent = RaiseTriggerEntered;

        if (raiseEvent is null) return;

        customEventArgs.Message += $" at {DateTime.Now}";
        raiseEvent(sender, customEventArgs);
    }

    private static void HandleCollision(ref TransformComponent left, ref TransformComponent right)
    {
        left.Velocity = Vector2.Zero;
        right.Velocity = Vector2.Zero;
    }

    private static bool AreColliding(Tuple<TransformComponent, RectangleColliderComponent> first,
        Tuple<TransformComponent, RectangleColliderComponent> second)
    {
        var left = BuildRectangle(first);
        var right = BuildRectangle(second);

        var intersect = Rectangle.Intersect(left, right);

        return !intersect.IsEmpty;

        Rectangle BuildRectangle(Tuple<TransformComponent, RectangleColliderComponent> target)
        {
            (TransformComponent transform, RectangleColliderComponent rectCollider) = target;

            return new Rectangle((int)(transform.Position.X + transform.Velocity.X),
                (int)(transform.Position.Y + transform.Velocity.Y),
                rectCollider.Size.Width,
                rectCollider.Size.Height);
        }
    }

    public void Dispose()
    {
    }
}
