namespace Scellecs.Morpeh.Extended;

public class Feature(World world, SystemsEngine systemsEngine)
{
    public World World { get; set; } = world;

    public Feature(World world, SystemsEngine systemsEngine, params IInitializer[] systems) : this(world, systemsEngine)
    {
        foreach (var system in systems) Add(system);
    }

    public void OnAwake()
    {
        systemsEngine.Initializers.Initialize();
        systemsEngine.Systems.Initialize();
        systemsEngine.FixedSystems.Initialize();
        systemsEngine.CleanupSystems.Initialize();
        systemsEngine.LateSystems.Initialize();
        systemsEngine.RenderSystems.Initialize();
    }

    public void OnUpdate(float deltaTime) => systemsEngine.Systems.Update(deltaTime);

    public void OnFixedUpdate(float deltaTime) => systemsEngine.FixedSystems.FixedUpdate(deltaTime);

    public void OnLateUpdate(float deltaTime)
    {
        systemsEngine.LateSystems.LateUpdate(deltaTime);
        systemsEngine.CleanupSystems.CleanupUpdate(deltaTime);
    }

    public void OnRender(float deltaTime) => systemsEngine.RenderSystems.Update(deltaTime);

    protected void Add(IInitializer initializer)
    {
        switch (initializer)
        {
            case IRenderSystem renderSystem:
                systemsEngine.RenderSystems.AddSystem(renderSystem);
                break;
            case ICleanupSystem cleanupSystem:
                systemsEngine.CleanupSystems.AddSystem(cleanupSystem);
                break;
            case ILateSystem lateSystem:
                systemsEngine.LateSystems.AddSystem(lateSystem);
                break;
            case IFixedSystem fixedSystem:
                systemsEngine.FixedSystems.AddSystem(fixedSystem);
                break;
            case ISystem system:
                systemsEngine.Systems.AddSystem(system);
                break;
            default:
                systemsEngine.Initializers.AddInitializer(initializer);
                break;
        }
    }

    public void Dispose()
    {
    }
}
