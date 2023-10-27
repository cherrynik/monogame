// namespace Entitas.Components.Data
// {
//     using Microsoft.Xna.Framework;
//
//     public class TransformComponent : IComponent
//     {
//         public Vector2 Position;
//         public Vector2 Velocity;
//     }
// }

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
