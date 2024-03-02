using Components.Data;
using Scellecs.Morpeh;

namespace Systems;

public class InventorySystem(World world) : ISystem
{
    public World World { get; set; } = world;

    public delegate void InventoryHandler(Entity sender, ref InventoryComponent inventory);

    public event InventoryHandler? RaiseInventoryInitialized;

    public void OnAwake()
    {
        var filter = World.Filter.With<InventoryComponent>().Build();

        if (filter is null) return;

        var entity = filter.First();
        ref var inventory = ref entity.GetComponent<InventoryComponent>();

        RaiseInventoryInitialized?.Invoke(entity, ref inventory);
    }

    public void OnUpdate(float deltaTime)
    {
    }

    public void Dispose()
    {
    }
}
