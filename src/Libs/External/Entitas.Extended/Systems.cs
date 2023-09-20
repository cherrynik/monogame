using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entitas.Extended;

// Entitas Extended Systems is compatible with methods linked to MonoGame Framework.
public class Systems : Entitas.Systems, IFixedExecuteSystem, IExecuteSystem, ILateExecuteSystem, IDrawSystem
{
    private readonly List<IFixedExecuteSystem> _fixedExecuteSystems = new();
    private readonly List<ILateExecuteSystem> _lateExecuteSystems = new();
    private new readonly List<IExecuteSystem> _executeSystems = new();
    private readonly List<IDrawSystem> _drawSystems = new();

    public override Entitas.Systems Add(ISystem system)
    {
        if (system is IFixedExecuteSystem fixedExecuteSystem)
            this._fixedExecuteSystems.Add(fixedExecuteSystem);
        if (system is IExecuteSystem executeSystem)
            this._executeSystems.Add(executeSystem);
        if (system is ILateExecuteSystem lateExecuteSystem)
            this._lateExecuteSystems.Add(lateExecuteSystem);
        if (system is IDrawSystem drawSystem)
            this._drawSystems.Add(drawSystem);
        return base.Add(system);
    }

    public new void Remove(ISystem system)
    {
        if (system is IFixedExecuteSystem fixedExecuteSystem)
            this._fixedExecuteSystems.Remove(fixedExecuteSystem);
        if (system is IExecuteSystem executeSystem)
            this._executeSystems.Remove(executeSystem);
        if (system is ILateExecuteSystem lateExecuteSystem)
            this._lateExecuteSystems.Remove(lateExecuteSystem);
        if (system is IDrawSystem drawSystem)
            this._drawSystems.Remove(drawSystem);
        base.Remove(system);
    }

    public void FixedExecute(GameTime gameTime)
    {
        for (int index = 0; index < this._fixedExecuteSystems.Count; ++index)
            this._fixedExecuteSystems[index].FixedExecute(gameTime);
    }

    public void Execute(GameTime gameTime)
    {
        for (int index = 0; index < this._executeSystems.Count; ++index)
            this._executeSystems[index].Execute(gameTime);
    }

    public void LateExecute(GameTime gameTime)
    {
        for (int index = 0; index < this._lateExecuteSystems.Count; ++index)
            this._lateExecuteSystems[index].LateExecute(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int index = 0; index < this._drawSystems.Count; ++index)
            this._drawSystems[index].Draw(gameTime, spriteBatch);
    }
}
