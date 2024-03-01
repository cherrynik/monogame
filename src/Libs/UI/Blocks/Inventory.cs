using Components.Data;
using FontStashSharp.RichText;
using Myra.Graphics2D.UI;

namespace UI.Blocks;

public class Inventory
{
    public Inventory(Container container, ref InventoryComponent inventory)
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
        container.Widgets.Add(title);

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
            container.Widgets.Add(label);
        }
    }
}
