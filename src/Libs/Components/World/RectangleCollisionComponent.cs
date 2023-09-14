using Entitas;
using Microsoft.Xna.Framework;
using Anchor = Services.Math.RadDir;

namespace Components.World;

public class RectangleCollisionComponent : IComponent
{
    public Rectangle Rectangle;
}
