using Scellecs.Morpeh;

namespace Components.Data;

// TODO: not sure if i should have ItemId.None (myb get rid of that and use null?)
public struct Slot()
{
    public int Amount { get; private set; }

    private ItemId _item = ItemId.None;

    public Slot(ItemId item, int amount) : this()
    {
        _item = item;
        Amount = amount;
    }

    public Item GetItemInfo() => ItemsTable.Items[_item];

    public Slot Put(ItemId item, int amount)
    {
        _item = item;
        Amount = amount;
        return this;
    }
}

public struct InventoryComponent(Slot[] slots) : IComponent
{
    public Slot[] Slots = slots;
}
