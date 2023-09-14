using Components.Sprites;
using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IExecuteSystem = Entitas.Extended.IExecuteSystem;

namespace Systems.Sprites;

public class MovementAnimatedSprites : IExecuteSystem, IDrawSystem
{
    private readonly IGroup<GameEntity> _group;

    public MovementAnimatedSprites(IGroup<GameEntity> group)
    {
        _group = group;
    }

    public void Execute(GameTime gameTime)
    {
        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            Vector2 velocity = e.transform.Velocity;
            Components.Sprites.MovementAnimatedSprites? movementAnimatedSprites =
                (Components.Sprites.MovementAnimatedSprites)e.GetComponent(0);

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

        GameEntity[]? entities = _group.GetEntities();
        foreach (GameEntity e in entities)
        {
            Vector2 position = e.transform.Position;
            Components.Sprites.MovementAnimatedSprites? movementAnimatedSprites =
                (Components.Sprites.MovementAnimatedSprites)e.GetComponent(0);

            movementAnimatedSprites.PlayingAnimation.Draw(spriteBatch, position);
        }

        spriteBatch.End();
    }
}
