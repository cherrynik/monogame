using Components.Data;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Render;

public class RenderFramesPerSec(Scellecs.Morpeh.World world) : IRenderSystem
{
    public Scellecs.Morpeh.World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        var world = World.Filter.With<WorldMetaComponent>().Build().First().GetComponent<WorldMetaComponent>();

        ImGui.Begin("Diagnostics",
            ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking);

        ImGui.Text($"FPS: {world.FramesPerSec:F2}");

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
