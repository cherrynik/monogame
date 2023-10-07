using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems;

public class CameraFollowingSystem : IDrawSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;

    public CameraFollowingSystem(Contexts contexts, IGroup<GameEntity> group)
    {
        _contexts = contexts;
        _group = group;
    }

    // todo: refactor, put the logic in impl
    // todo: smooth diagonal movement
    private Vector2 GetPosition(GameEntity target) =>
        new(
            (float)target.camera.Size.Width / 2 -
            (float)target.movementAnimation.PlayingAnimation.Width / 2,
            (float)target.camera.Size.Height / 2 -
            (float)target.movementAnimation.PlayingAnimation.Height / 2);

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        GameEntity[] entities = _group.GetEntities();

        spriteBatch.Begin(samplerState: SamplerState.PointWrap);

        var target = _contexts.game.cameraEntity;
        foreach (GameEntity e in entities)
        {
            Vector2 otherAt = e.transform.Position - (target?.transform.Position ?? Vector2.Zero);
            if (e.hasSprite)
            {
                e.sprite.Sprite.Draw(spriteBatch, otherAt);
                // todo: drawing complex entities' sprite/animated components
            }

            target?.movementAnimation.PlayingAnimation.Draw(spriteBatch, GetPosition(target));
        }

        spriteBatch.End();
    }
}
