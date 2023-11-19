using Components.Data;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging;

public class EntitiesList : IRenderSystem
{
    public World World { set; get; }

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

        ImGui.Begin("World");

        if (ImGui.CollapsingHeader("Entities"))
        {
            foreach (Entity e in filter) ImGui.Text(e.ToString());
        }

        ImGui.End();
    }

    public void Dispose()
    {
    }
}
