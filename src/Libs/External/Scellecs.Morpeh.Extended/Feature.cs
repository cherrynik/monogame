namespace Scellecs.Morpeh.Extended;

public class Feature(World world, SystemsEngine systemsEngine)
{
    public readonly SystemsEngine SystemsEngine = systemsEngine;
    public World World { get; set; } = world;

    public Feature(World world, SystemsEngine systemsEngine, params IInitializer[] systems) : this(world, systemsEngine)
    {
        foreach (var system in systems) Add(system);
    }

    public void OnAwake() => SystemsEngine.Initializers.Initialize();

    public virtual void OnUpdate(float deltaTime) => SystemsEngine.Systems.Update(deltaTime);

    public virtual void OnFixedUpdate(float deltaTime) => SystemsEngine.FixedSystems.FixedUpdate(deltaTime);

    public virtual void OnLateUpdate(float deltaTime)
    {
        SystemsEngine.LateSystems.LateUpdate(deltaTime);
        SystemsEngine.CleanupSystems.CleanupUpdate(deltaTime);
    }

    public virtual void OnRender(float deltaTime) => SystemsEngine.RenderSystems.Update(deltaTime);

    protected void Add(IInitializer initializer)
    {
        switch (initializer)
        {
            case IRenderSystem renderSystem:
                SystemsEngine.RenderSystems.AddSystem(renderSystem);
                break;
            case ICleanupSystem cleanupSystem:
                SystemsEngine.CleanupSystems.AddSystem(cleanupSystem);
                break;
            case ILateSystem lateSystem:
                SystemsEngine.LateSystems.AddSystem(lateSystem);
                break;
            case IFixedSystem fixedSystem:
                SystemsEngine.FixedSystems.AddSystem(fixedSystem);
                break;
            case ISystem system:
                SystemsEngine.Systems.AddSystem(system);
                break;
            default:
                SystemsEngine.Initializers.AddInitializer(initializer);
                break;
        }
    }

    public void Dispose()
    {
    }
}
