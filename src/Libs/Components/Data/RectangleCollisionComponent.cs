namespace Entitas.Components.Data
{
    using Microsoft.Xna.Framework;

    public class RectangleCollisionComponent : IComponent
    {
        public Rectangle Size;
    }
}

namespace Components.Data
{
    using Scellecs.Morpeh;
    using System.Drawing;

    public struct RectangleCollisionComponent : IComponent
    {
        public Rectangle Rectangle;
    }
}
