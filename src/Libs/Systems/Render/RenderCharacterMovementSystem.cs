using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Systems.Render;

public class RenderCharacterMovementSystem : ISystem
{
    private readonly SpriteBatch _spriteBatch;
    public World World { get; set; }

    public RenderCharacterMovementSystem(World world, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

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

            animator.Animation.Draw(_spriteBatch, transform.Position);
            animator.Animation.Update(deltaTime);

            AnimatedSprite animation;

            if (transform.Velocity.Equals(Vector2.Zero))
            {
                animation = animations.IdleAnimations[animator.Facing];
            }
            else
            {
                animator.Facing = MathUtils.Rad8DirYFlipped(transform.Velocity);
                animation = animations.WalkingAnimations[animator.Facing];
            }

            if (animator.Animation != animation)
            {
                animator.Animation = animation;
                animator.Animation.Play();
            }
        }
    }

    public void Dispose()
    {
    }
}
