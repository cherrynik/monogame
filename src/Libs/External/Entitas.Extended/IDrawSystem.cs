using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entitas.Extended;

public interface IDrawSystem : ISystem
{
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
