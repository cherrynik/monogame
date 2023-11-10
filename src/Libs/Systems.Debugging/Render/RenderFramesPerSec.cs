using Components.Data;
using ImGuiNET;
using Scellecs.Morpeh;

namespace Systems.Debugging.Render;

public class RenderFramesPerSec : ISystem
{
    public World World { get; set; }

    public RenderFramesPerSec(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        var world = World.Filter.With<WorldComponent>().Build().First().GetComponent<WorldComponent>();

        ImGui.Begin("Diagnostics");

        ImGui.Text($"FPS: {world.FramesPerSec}");

        ImGui.End();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
