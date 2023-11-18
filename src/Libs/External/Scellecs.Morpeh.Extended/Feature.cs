using Microsoft.Xna.Framework.Graphics;

namespace Scellecs.Morpeh.Extended;

public abstract class Feature
{
    public World World { get; set; }

    private readonly List<Feature> _features = new();
    private readonly SystemsGroup _initializers;
    private readonly SystemsGroup _systems;
    private readonly SystemsGroup _fixedSystems;
    private readonly SystemsGroup _lateSystems;
    private readonly SystemsGroup _cleanupSystems;
    private readonly SystemsGroup _renderSystems;

    public void OnAwake()
    {
        _initializers.Initialize();

        foreach (Feature feature in _features) feature._initializers.Initialize();
    }

    public virtual void OnUpdate(float deltaTime)
    {
        _systems.Update(deltaTime);

        foreach (Feature feature in _features) feature._systems.Update(deltaTime);
    }

    public virtual void OnFixedUpdate(float deltaTime)
    {
        _fixedSystems.FixedUpdate(deltaTime);

        foreach (Feature feature in _features) feature._fixedSystems.FixedUpdate(deltaTime);
    }

    public virtual void OnLateUpdate(float deltaTime)
    {
        _lateSystems.LateUpdate(deltaTime);
        _cleanupSystems.CleanupUpdate(deltaTime);

        foreach (Feature feature in _features)
        {
            feature._lateSystems.LateUpdate(deltaTime);
            feature._cleanupSystems.CleanupUpdate(deltaTime);
        }
    }

    public virtual void OnRender(float deltaTime)
    {
        _renderSystems.Update(deltaTime);

        foreach (Feature feature in _features) feature._renderSystems.Update(deltaTime);
    }

    protected Feature(World world)
    {
        World = world;

        _initializers = world.CreateSystemsGroup();
        _systems = world.CreateSystemsGroup();
        _fixedSystems = world.CreateSystemsGroup();
        _lateSystems = world.CreateSystemsGroup();
        _cleanupSystems = world.CreateSystemsGroup();
        _renderSystems = world.CreateSystemsGroup();
    }

    protected void Add(Feature feature) => _features.Add(feature);

    protected void Add(IInitializer initializer)
    {
        switch (initializer)
        {
            case IRenderSystem renderSystem:
                _renderSystems.AddSystem(renderSystem);
                break;
            case ICleanupSystem cleanupSystem:
                _cleanupSystems.AddSystem(cleanupSystem);
                break;
            case ILateSystem lateSystem:
                _lateSystems.AddSystem(lateSystem);
                break;
            case IFixedSystem fixedSystem:
                _fixedSystems.AddSystem(fixedSystem);
                break;
            case ISystem system:
                _systems.AddSystem(system);
                break;
            default:
                _initializers.AddInitializer(initializer);
                break;
        }
    }

    public void Dispose()
    {
    }
}
