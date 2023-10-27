using Entitas.Components.Data;
using Entitas;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Sprites;
using Serilog;
using Services.Math;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

public class AnimatedMovementSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly ILogger _logger;

    public AnimatedMovementSystem(IGroup<GameEntity> group, ILogger logger)
    {
        _group = group;
        _logger = logger;
    }

    // todo: null-checking everywhere
    public void Execute(GameTime gameTime)
    {
        GameEntity[] entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            Vector2 velocity = e.transform.Velocity;

            // TODO: refactor and put it in a service system?
            if (velocity.Equals(Vector2.Zero))
            {
                StopFacingTrace(e.movementAnimation);
            }
            else
            {
                MoveFacingTrace(e.movementAnimation, velocity);
            }

            e.movementAnimation.PlayingAnimation.Update(gameTime);
        }
    }

    private static void StopFacingTrace(MovementAnimationComponent component)
    {
        if (component.HasStopped)
        {
            return;
        }

        Direction direction = MathUtils.Rad8DirYFlipped(component.FacingDirection);

        component.PlayingAnimation = component.IdleAnimations[direction];
        component.PlayingAnimation.SetFrame(0);
        component.HasStopped = true;
    }

    private static void MoveFacingTrace(MovementAnimationComponent component, Vector2 velocity)
    {
        Direction direction = MathUtils.Rad8DirYFlipped(velocity);
        AnimatedSprite walkingAnimation = component.WalkingAnimations[direction];

        if (AreDifferentSprites(component.PlayingAnimation, walkingAnimation))
        {
            component.PlayingAnimation = walkingAnimation;
        }

        component.FacingDirection = velocity;
        component.HasStopped = false;
    }

    private static bool AreDifferentSprites(Sprite left, Sprite right) =>
        left.Name != right.Name ||
        left.FlipHorizontally != right.FlipHorizontally;
}
