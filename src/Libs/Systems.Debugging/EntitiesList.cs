using Components.Data;
using Scellecs.Morpeh;

namespace Systems.Debugging;

public class EntitiesList : ISystem
{
    public World World { get; set; }

    public EntitiesList(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();

        foreach (Entity e in filter)
        {
        }
    }

    public void Dispose()
    {
    }
}
