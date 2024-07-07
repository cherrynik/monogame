using Components.Data;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Diagnostics;

// Note:
// ISystem shows FPS of Update; IFixedSystem of FixedUpdate; IRenderSystem of Draw
public class FrameCounter(Scellecs.Morpeh.World world) : ISystem
{
    private const float UpdateFrequencyInSec = .0875f;
    private float _elapsedTime;
    private int _framesCount;
    private float _framesPerSecond;

    public Scellecs.Morpeh.World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<WorldMetaComponent>()
            .Build();

        if (filter.IsEmpty()) return;

        ref WorldMetaComponent worldMeta = ref filter
            .First()
            .GetComponent<WorldMetaComponent>();

        worldMeta.FramesPerSec = CalculateFps(deltaTime);
    }

    private float CalculateFps(float deltaTime)
    {
        ++_framesCount;
        _elapsedTime += deltaTime;

        if (_elapsedTime < UpdateFrequencyInSec) return _framesPerSecond;

        _framesPerSecond = _framesCount / _elapsedTime;

        _framesCount = 0;
        _elapsedTime = 0;

        return _framesPerSecond;
    }

    public void Dispose()
    {
    }
}
