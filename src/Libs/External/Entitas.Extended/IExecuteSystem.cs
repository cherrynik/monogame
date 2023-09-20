using Microsoft.Xna.Framework;

namespace Entitas.Extended;

public interface IExecuteSystem : ISystem
{
    void Execute(GameTime gameTime);
}
