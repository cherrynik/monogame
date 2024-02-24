using Scellecs.Morpeh;

namespace Components.Data;

public interface IItem
{
    ItemSettings ItemSettings { get; }
}

public struct ItemSettings(string name, bool isStackable)
{
    public string Name = name;
    public bool IsStackable = isStackable;
}

public struct Slot(IItem item)
{
    public IItem Item = item;
}

public struct InventoryComponent(Slot[] slots) : IComponent
{
    public Slot[] Slots = slots;
}
