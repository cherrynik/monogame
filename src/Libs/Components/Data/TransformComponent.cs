namespace Entitas.Components.Data
{
    using Microsoft.Xna.Framework;

    public class TransformComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Velocity;
    }
}

// Input Scan System -> Write Velocity

// Movement System -> Write Velocity
// Collision System -> Reset Velocity if there's collider in the velocity direction
// Position System -> Apply Velocity by changing Position

namespace Components.Data
{
    using Scellecs.Morpeh;
    using System.Numerics;

    public struct TransformComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Velocity;
    }
}
