using System.Numerics;

namespace Implementations.Camera;

public interface ICamera
{
    Vector2 Move(Vector2 from, Vector2 to, float step = 1);
}
