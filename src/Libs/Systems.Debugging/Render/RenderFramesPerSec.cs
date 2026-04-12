using Components.Data;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Render;

public class RenderFramesPerSec : IRenderSystem
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
        var worldComponentStash = World.GetStash<WorldComponent>();
        Entity worldEntity = default;
        bool hasWorldEntity = false;
        Filter worldFilter = World.Filter.With<WorldComponent>().Build();
        foreach (Entity entity in worldFilter)
        {
            worldEntity = entity;
            hasWorldEntity = true;
            break;
        }

        if (!hasWorldEntity || !worldComponentStash.Has(worldEntity))
        {
            return;
        }

        var world = worldComponentStash.Get(worldEntity);

        ImGui.Begin("Diagnostics");

        ImGui.Text($"FPS: {world.FramesPerSec}");

        ImGui.End();
    }

    public void Dispose()
    {
    }
}

// TODO: Tiles, save system, understand the fps logic
// UI Debug: entities inspector, memory graph, fps setter/limiter
// Testing ECS, workflow cleanup
// camera zoom, pause, scenes (menu, game), content (mechanics: quests, dialogues, details, tree chopping, etc)
// optimization, collisions, events, UI styling
// shaders
