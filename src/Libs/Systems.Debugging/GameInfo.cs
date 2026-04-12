namespace Systems.Debugging;

public readonly record struct GameInfo(
    bool IsFixedTimeStep,
    bool IsMouseVisible);
