using System.Numerics;
using Components.Data;
using Components.Render;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Implementations.Visuals;

public class CharacterAnimation
{
    public static void Update(Entity e, float deltaTime)
    {
        ref var animations = ref e.GetComponent<MovementAnimationsComponent>();
        ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();
        ref var transform = ref e.GetComponent<TransformComponent>();

        animator.Animation.Update(deltaTime);

        AnimatedSprite animation = GetAnimation(transform, animations, animator);
        animator.Facing = GetFacing(transform, animator);

        if (Equals(animator.Animation, animation)) return;

        animator.Animation = animation;
        // .Play() is called in the AnimatedCharactersFactory (on the step of creation);
        // otherwise, you'd call it manually here.
    }

    private static AnimatedSprite GetAnimation(TransformComponent transform,
        MovementAnimationsComponent animations,
        CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animations.IdleAnimations[animator.Facing]
            : animations.WalkingAnimations[animator.Facing];

    private static Sector GetFacing(TransformComponent transform, CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animator.Facing
            : MathUtils.VectorToSectorYFlipped(transform.Velocity);
}
