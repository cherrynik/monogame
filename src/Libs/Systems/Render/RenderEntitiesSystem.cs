using Components.Data;
using Implementations;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Scellecs.Morpeh.Extended.Extensions;

namespace Systems.Render;

public class RenderEntitiesSystem(World world, Rendering rendering) : IRenderSystem
{
    public World World { get; set; } = world;


    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter transformFilter = World.Filter.With<TransformComponent>().Build();
        if (transformFilter.IsEmpty()) return;

        Filter cameraFilter = World.Filter.With<CameraComponent>().Build();
        if (cameraFilter.IsEmpty()) return;
        var camera = cameraFilter.First().GetComponent<CameraComponent>();

        Filter worldMetaFilter = World.Filter.With<WorldMetaComponent>().Build();
        if (worldMetaFilter.IsEmpty()) return;
        ref var worldMeta = ref worldMetaFilter.First().GetComponent<WorldMetaComponent>();

        worldMeta.SortedEntities = EntitiesSorting.GetEntitiesSortedByY(transformFilter.AsEnumerableSlow());

        rendering.RenderEntities(worldMeta.SortedEntities, camera);
    }

    public void Dispose()
    {
    }
}
