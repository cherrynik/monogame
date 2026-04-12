using Components.Data;
using Scellecs.Morpeh;

namespace Systems.Debugging;

public class FrameCounter : ILateSystem
{
    private const float UpdateFrequencyInSec = .02f;
    private float _elapsedTime;
    private int _framesCount;

    public World World { get; set; }

    public FrameCounter(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        ++_framesCount;
        _elapsedTime += deltaTime;

        if (_elapsedTime < UpdateFrequencyInSec)
        {
            return;
        }

        var worldComponentStash = World.GetStash<WorldComponent>();
        Entity worldEntity = default;
        bool hasWorldEntity = false;
        Filter worldFilter = World.Filter.With<WorldComponent>().Build();
        foreach (Entity entity in worldFilter)
        {
            worldEntity = entity;
            hasWorldEntity = true;
            break;
        }

        if (!hasWorldEntity || !worldComponentStash.Has(worldEntity))
        {
            return;
        }

        ref var worldComponent = ref worldComponentStash.Get(worldEntity);
        worldComponent.FramesPerSec = _framesCount / _elapsedTime;

        _framesCount = 0;
        _elapsedTime = 0;
    }

    public void Dispose()
    {
    }
}
