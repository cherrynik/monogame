using Components.Data;
using Scellecs.Morpeh;

namespace Systems.Debugging;

public class FrameCounter(World world) : ISystem
{
    private const float UpdateFrequencyInSec = .02f;
    private float _elapsedTime;
    private int _framesCount;

    public World World { get; set; } = world;

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

        ref WorldMetaComponent worldMeta =
            ref World.Filter.With<WorldMetaComponent>().Build().First().GetComponent<WorldMetaComponent>();
        worldMeta.FramesPerSec = _framesCount / _elapsedTime;

        _framesCount = 0;
        _elapsedTime = 0;
    }

    public void Dispose()
    {
    }
}
