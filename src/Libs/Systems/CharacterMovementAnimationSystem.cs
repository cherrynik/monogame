using System.Numerics;
using Components.Data;
using Components.Render;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Systems;

public class CharacterMovementAnimationSystem(World world) : ISystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<CharacterAnimatorComponent>()
            .With<MovementAnimationsComponent>()
            .Build();

        foreach (Entity e in filter)
        {
            ref var animations = ref e.GetComponent<MovementAnimationsComponent>();
            ref var animator = ref e.GetComponent<CharacterAnimatorComponent>();
            ref var transform = ref e.GetComponent<TransformComponent>();

            animator.Animation.Update(deltaTime);

            AnimatedSprite animation = GetAnimation(transform, animations, animator);
            animator.Facing = GetDirection(transform, animator);

            if (animator.Animation == animation)
            {
                continue;
            }

            animator.Animation = animation;
            // .Play() is called in the AnimatedCharactersFactory (on the step of creation);
            // otherwise, you'd call it manually here.
        }
    }

    private static AnimatedSprite GetAnimation(TransformComponent transform,
        MovementAnimationsComponent animations,
        CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animations.IdleAnimations[animator.Facing]
            : animations.WalkingAnimations[animator.Facing];

    private static Sector GetDirection(TransformComponent transform, CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animator.Facing
            : MathUtils.VectorToSectorYFlipped(transform.Velocity);

    public void Dispose()
    {
    }
}
