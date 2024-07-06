using System.Numerics;

namespace Services.Implementations.Movement;

public class SimpleMovement : IMovement
{
    /// <param name="by">You won't see difference by passing the computed values through,
    /// as the parameter is only for a unit vector (e.g., velocity)</param>
    public Vector2 Move(Vector2 from, Vector2 by, float speed = 1)
    {
        if (by.Equals(Vector2.Zero)) return from;

        return from + Vector2.Normalize(by) * speed;
    }
}
