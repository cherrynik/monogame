using Scellecs.Morpeh;

namespace Components.Data;

// TODO: refactor here a lil bit
public enum ItemId
{
    None = 0,
    Rock = 1,
}

public readonly struct ItemComponent(ItemId itemId) : IComponent
{
    public ItemId ItemId { get; } = itemId;
}

public readonly struct Item(string name)
{
    public string Name { get; } = name;
    public int MaximumInStack { get; } = 1;
    public bool IsStackable { get; } = false;

    public Item(string name, int maximumInStack) : this(name)
    {
        Name = name;
        MaximumInStack = maximumInStack;

        IsStackable = maximumInStack switch
        {
            > 1 => true,
            < 1 => throw new ArgumentOutOfRangeException(nameof(maximumInStack), maximumInStack,
                "Has to be >= 1"),
            _ => IsStackable
        };
    }
}

// TODO: put in data tables namespace (?)
public static class ItemsTable
{
    public static readonly Dictionary<ItemId, Item> Items = new()
    {
        { ItemId.None, new Item(name: "None") }, { ItemId.Rock, new Item(name: "Rock", maximumInStack: 16) }
    };
}
