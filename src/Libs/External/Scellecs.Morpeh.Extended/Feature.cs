using Microsoft.Xna.Framework.Graphics;

namespace Scellecs.Morpeh.Extended;

public abstract class Feature : ISystem
{
    public World World { get; set; }

    protected SystemsGroup _initializers;
    protected SystemsGroup _systems;
    protected SystemsGroup _fixedSystems;
    protected SystemsGroup _lateSystems;
    protected SystemsGroup _cleanupSystems;
    protected SystemsGroup _renderSystems;

    public Feature(World world)
    {
        World = world;

        _initializers = world.CreateSystemsGroup();
        _systems = world.CreateSystemsGroup();
        _fixedSystems = world.CreateSystemsGroup();
        _lateSystems = world.CreateSystemsGroup();
        _cleanupSystems = world.CreateSystemsGroup();
        _renderSystems = world.CreateSystemsGroup();
    }

    protected void Add(IInitializer initializer)
    {
        if (initializer is ICleanupSystem cleanupSystem) _cleanupSystems.AddSystem(cleanupSystem);

        if (initializer is ILateSystem lateSystem) _lateSystems.AddSystem(lateSystem);

        if (initializer is IFixedSystem fixedSystem) _fixedSystems.AddSystem(fixedSystem);

        if (initializer is ISystem system) _systems.AddSystem(system);

        if (initializer is IRenderSystem renderSystem) _renderSystems.AddSystem(renderSystem);

        _initializers.AddInitializer(initializer);
    }

    public void OnAwake()
    {
        _initializers.Initialize();
    }

    public void OnUpdate(float deltaTime)
    {
        _systems.Update(deltaTime);
    }

    public void OnFixedUpdate(float deltaTime)
    {
        _fixedSystems.Update(deltaTime);
    }

    public void OnLateUpdate(float deltaTime)
    {
        _lateSystems.Update(deltaTime);
    }

    public void OnRender(float deltaTime)
    {
        _renderSystems.Update(deltaTime);
    }

    public void Dispose()
    {
        _cleanupSystems.Dispose();
    }
}
