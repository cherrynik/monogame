using Components.Data;
using Entities.Factories.Meta;
using ImGuiNET;
using LightInject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Render;

public class RenderFramesPerSec(Scellecs.Morpeh.World world, IServiceFactory serviceFactory) : IRenderSystem
{
    public Scellecs.Morpeh.World World { get; set; } = world;

    public void OnAwake()
    {
        var worldEntityFactory = serviceFactory.GetInstance<WorldEntityFactory>();
        worldEntityFactory.CreateEntity(World);
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<WorldMetaComponent>().Build();

        if (filter.IsEmpty()) return;

        var world = filter.First().GetComponent<WorldMetaComponent>();

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

// TODO: save system, understand the fps logic
// UI Debug: memory graph, fps setter/limiter, viewports
// camera zoom, pause, scenes (menu, game), content (mechanics: quests, dialogues, details, tree chopping, etc)
// optimization, UI styling, shaders
