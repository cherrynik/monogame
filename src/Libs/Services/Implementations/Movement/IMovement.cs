using System.Numerics;

namespace Services.Implementations.Movement;

public interface IMovement
{
    Vector2 Move(Vector2 from, Vector2 by, float speed);
}
