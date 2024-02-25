using Scellecs.Morpeh;

namespace Components.Data;

public enum ItemId
{
    Rock = 0,
}

public readonly struct ItemComponent(ItemId itemId) : IComponent
{
    public ItemId ItemId { get; } = itemId;
}

public readonly struct Item(string name, bool isStackable, int? maximumInStack)
{
    public string Name { get; } = name;
    public bool IsStackable { get; } = isStackable;
    public int MaximumInStack { get; } = maximumInStack ?? 1;
}
