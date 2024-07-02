using Microsoft.Xna.Framework;
using Scellecs.Morpeh;

namespace Components.Data;

public struct RectangleColliderComponent : IComponent
{
    public Rectangle Size;
    public TransformComponent LocalTransform;
    public bool IsTrigger;
}
