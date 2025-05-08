using Scellecs.Morpeh;

namespace Components.Data;

// TODO: not sure if i should have ItemId.None (myb get rid of that and use null?)
public struct Slot()
{
    public ItemId Item = ItemId.None;
    public int Amount;

    public Slot(ItemId item, int amount) : this()
    {
        Item = item;
        Amount = amount;
    }

    public Item GetInfo() => ItemsTable.Items[Item];
}

public struct InventoryComponent(Slot[] slots) : IComponent
{
    public Slot[] Slots = slots;
}
