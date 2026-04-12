using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Systems.Render;

public class CharacterMovementAnimationSystem : ISystem
{
    private Filter _filter = default!;
    public World World { get; set; }

    public CharacterMovementAnimationSystem(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
        _filter = World.Filter
            .With<CharacterAnimatorComponent>()
            .With<MovementAnimationsComponent>()
            .Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var movementAnimationsStash = World.GetStash<MovementAnimationsComponent>();
        var characterAnimatorStash = World.GetStash<CharacterAnimatorComponent>();
        var transformStash = World.GetStash<TransformComponent>();

        foreach (Entity e in _filter)
        {
            ref var animations = ref movementAnimationsStash.Get(e);
            ref var animator = ref characterAnimatorStash.Get(e);
            ref var transform = ref transformStash.Get(e);

            // 2. And this one could be in the draw state
            // animator.Animation.Draw(_spriteBatch, transform.Position);

            // 1. Actually, all of this could be put in the pre-draw
            animator.Animation.Update(deltaTime);

            AnimatedSprite animation = GetAnimation(transform, animations, animator);
            animator.Facing = GetDirection(transform, animator);

            if (animator.Animation == animation)
            {
                continue;
            }

            animator.Animation = animation;
            // .Play() is called in the AnimatedCharactersFactory (on the step of creation),
            // otherwise, you'd call it manually here.
        }
    }

    private static AnimatedSprite GetAnimation(TransformComponent transform,
        MovementAnimationsComponent animations,
        CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animations.IdleAnimations[animator.Facing]
            : animations.WalkingAnimations[animator.Facing];

    private static Direction GetDirection(TransformComponent transform, CharacterAnimatorComponent animator) =>
        transform.Velocity.Equals(Vector2.Zero)
            ? animator.Facing
            : MathUtils.Rad8DirYFlipped(transform.Velocity);

    public void Dispose()
    {
    }
}
