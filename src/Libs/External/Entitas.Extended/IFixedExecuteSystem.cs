using Microsoft.Xna.Framework;

namespace Entitas.Extended;

public interface IFixedExecuteSystem : ISystem
{
    void FixedExecute(GameTime gameTime);
}
