using Microsoft.Xna.Framework;

namespace Services.Movement;

public class SimpleMovement : ISafeMovement
{
    public Vector2 Move(Vector2 from, Vector2 by) => from + Vector2.Normalize(by);

    public Vector2 SafeMove(Vector2 from, Vector2 by) => by.Equals(Vector2.Zero) ? from : Move(from, by);
}
