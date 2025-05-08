using Components.Data;
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

    // TODO: Use graph instead (e.g. QuikGraph library), so will be handled the cases of relations like (combinatorics):
    // { 1: [2, 3] }, { 2: [1, 2] }, { 3: [1] }, { 4: [] }
    private (EntityId, EntityId)[] _activeIntersect = [];

    public delegate void OutsideHandler(Entity sender, Entity with);

    public delegate void InsideHandler(Entity sender, Entity with, bool isTriggerEvent);

    // https://stackoverflow.com/questions/803242/understanding-events-and-event-handlers-in-c-sharp#:~:text=For%20completeness%27%20sake,fun%22%20NullReferenceException%20there.
    public event InsideHandler? Entered;
    public event InsideHandler? Stay;
    public event OutsideHandler? Exited;

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

                var intersect = Intersect(new(leftTransform, leftCollider),
                    new(rightTransform, rightCollider));
                var entities = (e.ID, other.ID);
                if (!intersect)
                {
                    if (_activeIntersect.Contains(entities))
                    {
                        _activeIntersect = _activeIntersect
                            .TakeWhile(x => x != entities)
                            .ToArray();

                        Exited?.Invoke(e, other);
                    }

                    continue;
                }

                var isTriggerEvent = leftCollider.IsTrigger || rightCollider.IsTrigger;
                if (!isTriggerEvent) OnCollision(ref leftTransform, ref rightTransform);

                if (_activeIntersect.Contains(entities) || _activeIntersect.Contains((entities.Item2, entities.Item1)))
                {
                    Stay?.Invoke(e, other, isTriggerEvent);
                    continue;
                }

                _activeIntersect = _activeIntersect
                    .TakeWhile(x => x != entities || x != (entities.Item2, entities.Item1))
                    .Append(entities)
                    .ToArray();

                Entered?.Invoke(e, other, isTriggerEvent);
            }
        }
    }


    private static void OnCollision(ref TransformComponent left, ref TransformComponent right)
    {
        left.Velocity = Vector2.Zero;
        right.Velocity = Vector2.Zero;
    }

    private static bool Intersect(Tuple<TransformComponent, RectangleColliderComponent> first,
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
