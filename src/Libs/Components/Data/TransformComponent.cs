using System.Numerics;
using Scellecs.Morpeh;
using Services.Implementations.Math;

namespace Components.Data;

public struct TransformComponent : IComponent
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Sector Pivot; // Shouldn't be set in DI, as it's registered at LDtk

    public Vector2 GetOffPivot(float w, float h) => GetOffPivot(w, h, Pivot);

    private static Vector2 GetOffPivot(float w, float h, Sector sector)
    {
        var pivot = MathUtils.SectorToVector(sector);

        return new Vector2(
            (pivot.X + 1) * w / 2,
            (-pivot.Y + 1) * h / 2
        );
    }
}

// Input Scan System -> Write Velocity

// Movement System -> Write Velocity
// Collision System -> Reset Velocity if there's collider in the velocity direction
// Position System -> Apply Velocity by changing Position
