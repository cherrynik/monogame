using System.Numerics;

namespace Systems.Input.Abstractions;

public interface IPhysicalKeyboardSource
{
    bool TryGetDirection(out Vector2 direction);
}
