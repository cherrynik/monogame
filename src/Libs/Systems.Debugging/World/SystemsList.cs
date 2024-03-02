using ImGuiNET;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Extended;
using Scellecs.Morpeh.Extended.Extensions;

namespace Systems.Debugging.World;

public class SystemsList(Scellecs.Morpeh.World world, SystemsEngine systemsEngine) : IRenderSystem
{
    public Scellecs.Morpeh.World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        ImGui.Begin("World");

        if (!ImGui.TreeNode($"Systems ({systemsEngine.GetLengthSlow()})"))
        {
            return;
        }

        DrawSystemsGroup("1. Initializers", systemsEngine.Initializers.GetInitializersInfo());

        DrawSystemsGroup("2. FixedSystems", systemsEngine.FixedSystems.GetFixedSystemsInfo());
        DrawSystemsGroup("3. Systems", systemsEngine.Systems.GetSystemsInfo());
        DrawSystemsGroup("4. LateSystems", systemsEngine.LateSystems.GetLateSystemsInfo());

        DrawSystemsGroup("5. RenderSystems", systemsEngine.RenderSystems.GetSystemsInfo());

        DrawSystemsGroup("[Undetermined] CleanupSystems", systemsEngine.CleanupSystems.GetCleanupSystemsInfo());

        ImGui.TreePop();
    }

    private static void DrawSystemsGroup<T>(string name, FastList<T> items)
    {
        if (!ImGui.TreeNode($"{name} ({items.length})"))
        {
            return;
        }

        foreach (var system in items.data)
        {
            if (system is null) continue;

            ImGui.TextWrapped(system.ToString());
        }

        ImGui.TreePop();
    }

    public void Dispose()
    {
    }
}
