using System.Numerics;

namespace Implementations.Camera;

public class Camera : ICamera
{
    public Vector2 Move(Vector2 from, Vector2 to, float step) => Vector2.Lerp(from, to, step);
}
