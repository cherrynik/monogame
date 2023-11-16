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

// TODO: Tiles, save system, understand the fps logic
// UI Debug: entities inspector, memory graph, fps setter/limiter
// Testing ECS, workflow cleanup
// camera zoom, pause, scenes (menu, game), content (mechanics: quests, dialogues, details, tree chopping, etc)
// optimization, collisions, events, UI styling
// shaders
