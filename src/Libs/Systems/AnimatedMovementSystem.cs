using Components.Sprites;
using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems;

public class AnimatedMovementSystem : IExecuteSystem, IDrawSystem
{
    private readonly IGroup<GameEntity> _group;
    private readonly ILogger _logger;

    public AnimatedMovementSystem(IGroup<GameEntity> group, ILogger logger)
    {
        _group = group;
        _logger = logger;
    }

    public void Execute(GameTime gameTime)
    {
        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            Vector2 velocity = e.transform.Velocity;
            AnimatedMovementComponent movementAnimatedSprites = e.animatedMovement;

            // TODO: refactor and put it in service system
            if (velocity.Equals(Vector2.Zero))
            {
                movementAnimatedSprites.StateMachine.Fire(PlayerTrigger.Stop);
            }
            else
            {
                movementAnimatedSprites.StateMachine.Fire(movementAnimatedSprites.MoveWithParameters, velocity);
            }

            movementAnimatedSprites.PlayingAnimation.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        GameEntity[] entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            Vector2 position = e.transform.Position;
            AnimatedMovementComponent movementAnimatedSprites = e.animatedMovement;

            movementAnimatedSprites.PlayingAnimation.Draw(spriteBatch, position);
        }

        spriteBatch.End();
    }
}
