using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;

namespace Components.Render.Static;

public readonly struct SpriteComponent(Sprite sprite, TransformComponent transform) : IComponent, IPivotted
{
    public Sprite Sprite { get; } = sprite;
    public TransformComponent LocalTransform { get; } = transform;

    public Vector2 GetOffPivot(TransformComponent entityTransform) =>
        entityTransform.GetOffPivot(Sprite.Width, Sprite.Height);
}
