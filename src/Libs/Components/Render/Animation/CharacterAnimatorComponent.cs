using System.Numerics;
using Components.Data;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render.Animation;

public interface IPivotted
{
    public Vector2 GetOffPivot(TransformComponent entityTransform);
}

public struct CharacterAnimatorComponent(Sector facing, AnimatedSprite animation, TransformComponent transform)
    : IComponent, IPivotted
{
    public Sector Facing = facing;
    public AnimatedSprite Animation = animation;
    public TransformComponent LocalTransform { get; } = transform;

    public Vector2 GetOffPivot(TransformComponent entityTransform) =>
        entityTransform.GetOffPivot(Animation.Width, Animation.Height);
}
