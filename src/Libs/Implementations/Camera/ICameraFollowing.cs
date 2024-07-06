using System.Numerics;

namespace Implementations.Camera;

public interface ICameraFollowing : ICamera
{
    Vector2 Move(Vector2 from, Vector2 by);
}
