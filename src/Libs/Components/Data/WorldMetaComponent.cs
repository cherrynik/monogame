using Microsoft.Xna.Framework;
using Scellecs.Morpeh;

namespace Components.Data;

public struct WorldMetaComponent(Vector2 borderLimitsByAxis) : IComponent
{
    public static readonly Vector2 ZeroPosition = Vector2.Zero;
    public readonly Vector2 BorderLimitsByAxis = borderLimitsByAxis;
    public IEnumerable<Entity> SortedEntities;

    public float FramesPerSec;
}
