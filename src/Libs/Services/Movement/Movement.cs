using Microsoft.Xna.Framework;

namespace Services.Movement;

public class SimpleMovement : IMovement
{
    public Vector2 Move(Vector2 from, Vector2 by) => from + Vector2.Normalize(by);
}
