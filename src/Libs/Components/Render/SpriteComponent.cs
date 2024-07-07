using System.Numerics;
using Components.Data;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;

namespace Components.Render;

public readonly struct SpriteComponent(Sprite sprite, TransformComponent localTransform) : IComponent, IPivotted
{
    public Sprite Sprite { get; } = sprite;
    public TransformComponent LocalTransform { get; } = localTransform;

    public Vector2 GetOffPivot(TransformComponent parentTransform) =>
        parentTransform.GetOffPivot(Sprite.Width, Sprite.Height);
}
