using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems;

public class CameraFollowingSystem : IDrawSystem
{
    private readonly GameEntity? _target;
    private readonly IGroup<GameEntity> _group;

    public CameraFollowingSystem(GameEntity? target, IGroup<GameEntity> group)
    {
        _target = target;
        _group = group;
    }

    // todo: smooth diagonal movement
    private Vector2 GetTargetPosition()
    {
        if (_target is null)
        {
            return Vector2.Zero;
        }

        return new Vector2(
            (float)_target.camera.Size.Width / 2 - (float)_target.movementAnimation.PlayingAnimation.Width / 2,
            (float)_target.camera.Size.Height / 2 - (float)_target.movementAnimation.PlayingAnimation.Height / 2);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        GameEntity[] entities = _group.GetEntities();

        spriteBatch.Begin(samplerState: SamplerState.PointWrap);
        foreach (GameEntity e in entities)
        {
            Vector2 otherAt = e.transform.Position - (_target?.transform.Position ?? Vector2.Zero);
            if (e.hasSprite)
            {
                e.sprite.Sprite.Draw(spriteBatch, otherAt);
                // todo: drawing complex entities' sprite/animated components
            }

            _target?.movementAnimation.PlayingAnimation.Draw(spriteBatch, GetTargetPosition());
        }

        spriteBatch.End();
    }
}
