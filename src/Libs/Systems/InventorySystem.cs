using Components.Data;
using Scellecs.Morpeh;

namespace Systems;

public class InventorySystem(World world) : ISystem
{
    public World World { get; set; } = world;

    public delegate void InitializeHandler(Entity sender, ref InventoryComponent inventory);

    public event InitializeHandler? InventoryInitialized;

    public void OnAwake()
    {
        var filter = World.Filter.With<InventoryComponent>().Build();

        if (filter.IsEmpty()) return;

        var entity = filter.First();
        ref var inventory = ref entity.GetComponent<InventoryComponent>();

        InventoryInitialized?.Invoke(entity, ref inventory);
    }

    public void OnUpdate(float deltaTime)
    {
    }

    public void Dispose()
    {
    }
}
