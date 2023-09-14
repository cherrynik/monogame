using Microsoft.Xna.Framework;

namespace Services;

public interface ISafeMovement : IMovement
{
    Vector2 SafeMove(Vector2 from, Vector2 by);
}
