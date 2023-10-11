using Entitas;
using Entitas.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems;

public class DefaultDrawSystem : IDrawSystem
{
    private readonly IGroup<GameEntity> _group;

    public DefaultDrawSystem(IGroup<GameEntity> group)
    {
        _group = group;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // todo: refactor, put it in an impl and use the impl as a dep
        GameEntity[] entities = _group.GetEntities().OrderBy(x => x.transform.Position.Y).ToArray();

        spriteBatch.Begin(samplerState: SamplerState.PointWrap);

        foreach (GameEntity e in entities)
        {
            var position = e.transform.Position;
            // todo: refactor, put it in an impl and use the impl as a dep
            if (e.hasSprite)
            {
                e.sprite.Sprite?.Draw(spriteBatch, position);
            }

            if (e.hasMovementAnimation)
            {
                e.movementAnimation.PlayingAnimation?.Draw(spriteBatch, position);
            }
        }

        spriteBatch.End();
    }
}
