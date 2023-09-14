using Microsoft.Xna.Framework;

namespace Entitas.Extended;

public interface ILateExecuteSystem : ISystem
{
    void LateExecute(GameTime gameTime);
}
