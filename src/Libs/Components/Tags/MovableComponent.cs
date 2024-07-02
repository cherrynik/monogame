using Scellecs.Morpeh;

namespace Components.Tags;

public struct MovableComponent(float speed) : IComponent
{
    public float Speed = speed;
};
