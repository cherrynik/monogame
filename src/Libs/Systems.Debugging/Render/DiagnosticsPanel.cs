using System.Numerics;
using Components.Data;
using Components.Tags;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Pivot = Services.Math.Direction;

namespace Systems.Debugging.Render;

public class DiagnosticsPanel : IRenderSystem
{
    private const int FpsSampleCount = 120;

    private readonly GraphicsDevice _graphicsDevice;
    private readonly GameInfo _gameInfo;
    private readonly float[] _fpsSamples = new float[FpsSampleCount];
    private int _fpsSampleIndex;

    private Filter _worldFilter = default!;
    private Filter _cameraFilter = default!;
    private Filter _playerFilter = default!;
    private Filter _allEntitiesFilter = default!;

    public World World { get; set; }

    public DiagnosticsPanel(World world, GraphicsDevice graphicsDevice, GameInfo gameInfo)
    {
        World = world;
        _graphicsDevice = graphicsDevice;
        _gameInfo = gameInfo;
    }

    public void OnAwake()
    {
        DebugTheme.EnsureApplied();
        _worldFilter = World.Filter.With<WorldComponent>().Build();
        _cameraFilter = World.Filter.With<CameraComponent>().Build();
        _playerFilter = World.Filter.With<InputMovableComponent>().With<TransformComponent>().Build();
        _allEntitiesFilter = World.Filter.With<TransformComponent>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        ImGui.Begin("Diagnostics");

        DrawPerformance(deltaTime);
        DrawWorldInfo();
        DrawCameraInfo();
        DrawPlayerInfo();
        DrawGameState();

        ImGui.End();
    }

    public void Dispose()
    {
    }

    private void DrawPerformance(float deltaTime)
    {
        if (!Section("Performance", DebugTheme.Accent))
        {
            return;
        }

        float fps = 0f;
        var worldStash = World.GetStash<WorldComponent>();
        foreach (Entity e in _worldFilter)
        {
            if (worldStash.Has(e))
            {
                fps = worldStash.Get(e).FramesPerSec;
            }

            break;
        }

        _fpsSamples[_fpsSampleIndex] = fps;
        _fpsSampleIndex = (_fpsSampleIndex + 1) % FpsSampleCount;

        Vector4 fpsColor = fps >= 55 ? DebugTheme.Good : fps >= 30 ? DebugTheme.Warn : DebugTheme.CollisionColor;
        ImGui.TextColored(fpsColor, $"{fps:F0} FPS");
        ImGui.SameLine();
        Dim($"({deltaTime * 1000f:F1} ms)");

        ImGui.PlotLines("##fps", ref _fpsSamples[0], FpsSampleCount, _fpsSampleIndex,
            null, 0f, 120f, new Vector2(ImGui.GetContentRegionAvail().X, 32));

        ImGui.Separator();
    }

    private void DrawWorldInfo()
    {
        if (!Section("World", DebugTheme.CameraColor))
        {
            return;
        }

        int entityCount = 0;
        foreach (Entity _ in _allEntitiesFilter)
        {
            entityCount++;
        }

        Pair("Entities", $"{entityCount}");
        ImGui.Separator();
    }

    private void DrawCameraInfo()
    {
        if (!Section("Camera", DebugTheme.CameraEntityColor))
        {
            return;
        }

        var cameraStash = World.GetStash<CameraComponent>();
        foreach (Entity e in _cameraFilter)
        {
            if (!cameraStash.Has(e))
            {
                continue;
            }

            ref var cam = ref cameraStash.Get(e);
            Pair("Position", $"{cam.Position.X:F1}, {cam.Position.Y:F1}");
            Pair("Viewport", $"{cam.Viewport.Width} x {cam.Viewport.Height}");
            break;
        }

        ImGui.Separator();
    }

    private void DrawPlayerInfo()
    {
        if (!Section("Player", DebugTheme.PlayerColor))
        {
            return;
        }

        var transformStash = World.GetStash<TransformComponent>();
        foreach (Entity e in _playerFilter)
        {
            if (!transformStash.Has(e))
            {
                continue;
            }

            ref var t = ref transformStash.Get(e);
            Pair("Position", $"{t.Position.X:F1}, {t.Position.Y:F1}");
            Pair("Velocity", $"{t.Velocity.X:F1}, {t.Velocity.Y:F1}");
            Pair("Facing", $"{t.Pivot}");
            break;
        }

        ImGui.Separator();
    }

    private void DrawGameState()
    {
        if (!Section("Game", DebugTheme.InventoryColor))
        {
            return;
        }

        Pair("Window", $"{_graphicsDevice.Viewport.Width} x {_graphicsDevice.Viewport.Height}");
        Pair("FixedStep", $"{_gameInfo.IsFixedTimeStep}");
        Pair("Mouse", $"{_gameInfo.IsMouseVisible}");
    }

    private static bool Section(string label, Vector4 color)
    {
        ImGui.PushStyleColor(ImGuiCol.Text, color);
        bool open = ImGui.CollapsingHeader(label, ImGuiTreeNodeFlags.DefaultOpen);
        ImGui.PopStyleColor();
        return open;
    }

    private static void Pair(string label, string value)
    {
        ImGui.TextColored(DebugTheme.TextDim, $"  {label}:");
        ImGui.SameLine();
        ImGui.Text(value);
    }

    private static void Dim(string text)
    {
        ImGui.TextColored(DebugTheme.TextDim, text);
    }
}
