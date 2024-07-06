using Scellecs.Morpeh;

namespace Components.Data;

public struct MovableComponent(float speedUnits) : IComponent
{
    public readonly float Speed = speedUnits;
};
