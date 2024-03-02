using Scellecs.Morpeh;

namespace Components.Data;

public readonly struct NameComponent(string name) : IComponent
{
    public string Name { get; } = name;
}
