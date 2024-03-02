using Components.Data;
using FontStashSharp.RichText;
using Myra.Graphics2D.UI;
using Scellecs.Morpeh;
using Systems;

namespace UI.Blocks;

public class Inventory
{
    private readonly Container _container;
    private readonly InventorySystem _inventorySystem;

    public Inventory(Container container, InventorySystem inventorySystem)
    {
        _container = container;
        _inventorySystem = inventorySystem;

        _inventorySystem.InventoryInitialized += OnInventoryInitialized;
    }

    private void OnInventoryInitialized(Entity sender, ref InventoryComponent inventory)
    {
        var title = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            TextAlign = TextHorizontalAlignment.Left,
            Text = "Inventory",
            Top = 20,
            Left = 30,
        };
        _container.Widgets.Add(title);

        for (int i = 0; i < inventory.Slots.Length; i++)
        {
            var slot = inventory.Slots[i];
            var label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextAlign = TextHorizontalAlignment.Left,
                Text = $"{i + 1}: {slot.GetInfo().Name}, {slot.Amount}",
                Top = 50 + 18 * i,
                Left = 30,
            };
            _container.Widgets.Add(label);
        }
    }
}
