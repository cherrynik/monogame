using Components.Data;
using Scellecs.Morpeh;

namespace Systems.Debugging;

public class FrameCounter : ISystem
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

        World.Filter.With<WorldComponent>().Build().First().GetComponent<WorldComponent>().FramesPerSec =
            _framesCount / _elapsedTime;

        _framesCount = 0;
        _elapsedTime = 0;
    }

    public void Dispose()
    {
    }
}
