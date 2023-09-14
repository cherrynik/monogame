using Microsoft.Xna.Framework;

namespace Services;

public interface IMovement
{
    Vector2 Move(Vector2 from, Vector2 by);
}
