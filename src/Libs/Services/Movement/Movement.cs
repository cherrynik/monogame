using System.Numerics;

namespace Services.Movement;

public class SimpleMovement : IMovement
{
    public Vector2 Move(Vector2 from, Vector2 by)
    {
        if (by.Equals(Vector2.Zero))
        {
            return from;
        }

        return from + Vector2.Normalize(by);
    }
}
