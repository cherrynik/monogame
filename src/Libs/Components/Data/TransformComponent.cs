using Microsoft.Xna.Framework;
using Scellecs.Morpeh;

namespace Components.Data;

public struct TransformComponent : IComponent
{
    public Vector2 Position;
    public Vector2 Velocity;
}

// Input Scan System -> Write Velocity

// Movement System -> Write Velocity
// Collision System -> Reset Velocity if there's collider in the velocity direction
// Position System -> Apply Velocity by changing Position
