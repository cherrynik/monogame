using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems;

public class CameraFollowingSystem : IDrawSystem
{
    private readonly GameEntity _target;
    private readonly IGroup<GameEntity> _group;

    public CameraFollowingSystem(GameEntity target, IGroup<GameEntity> group)
    {
        _target = target;
        _group = group;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        GameEntity[] entities = _group.GetEntities();

        spriteBatch.Begin();
        foreach (GameEntity e in entities)
        {
            Vector2 targetAt =
                new Vector2(_target.camera.Size.Width / 2 - _target.movementAnimation.PlayingAnimation.Width / 2,
                    _target.camera.Size.Height / 2 - _target.movementAnimation.PlayingAnimation.Height / 2);

            Vector2 otherAt = e.transform.Position - _target.transform.Position;
            if (e.hasSprite)
            {
                e.sprite.Sprite.Draw(spriteBatch, otherAt);
            }

            _target.movementAnimation.PlayingAnimation.Draw(spriteBatch, targetAt);
        }

        spriteBatch.End();
    }
}
