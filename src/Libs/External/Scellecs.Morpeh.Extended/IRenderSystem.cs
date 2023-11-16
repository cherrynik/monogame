using Microsoft.Xna.Framework.Graphics;

namespace Scellecs.Morpeh.Extended;

public interface IRenderSystem : ISystem, IInitializer, IDisposable
{
    void OnRender(float deltaTime, SpriteBatch spriteBatch);
}
