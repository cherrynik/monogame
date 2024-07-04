using Scellecs.Morpeh;

namespace Components.Tags;

public struct MovableComponent(float speedUnits) : IComponent
{
    public readonly float Speed = speedUnits;
};
