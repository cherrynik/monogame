using Microsoft.Xna.Framework;

namespace Mechanics;

public interface IMovement
{
    Vector2 Move(Vector2 from, Vector2 by);
}

public class SimpleMovement : IMovement
{
    public Vector2 Move(Vector2 from, Vector2 by) => from + by;
}
