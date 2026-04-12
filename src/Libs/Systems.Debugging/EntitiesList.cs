using System.Numerics;
using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging;

public class EntitiesList : IRenderSystem
{
    public World World { set; get; }
    private Filter _transformFilter = default!;

    public EntitiesList(World world) => World = world;

    public void OnAwake()
    {
        DebugTheme.EnsureApplied();
        _transformFilter = World.Filter.With<TransformComponent>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        var inventoryStash = World.GetStash<InventoryComponent>();
        var transformStash = World.GetStash<TransformComponent>();
        var cameraStash = World.GetStash<CameraComponent>();
        var rectangleCollisionStash = World.GetStash<RectangleCollisionComponent>();
        var characterAnimatorStash = World.GetStash<CharacterAnimatorComponent>();
        var movementAnimationsStash = World.GetStash<MovementAnimationsComponent>();
        var spriteStash = World.GetStash<SpriteComponent>();
        var inputMovableStash = World.GetStash<InputMovableComponent>();
        var movableStash = World.GetStash<MovableComponent>();
        var renderStash = World.GetStash<RenderComponent>();

        ImGui.Begin("World");

        int entityCount = 0;
        foreach (Entity _ in _transformFilter)
        {
            entityCount++;
        }

        if (!ImGui.CollapsingHeader($"Entities ({entityCount})", ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGui.End();
            return;
        }

        foreach (Entity e in _transformFilter)
        {
            bool isPlayer = inputMovableStash.Has(e);
            bool isCamera = cameraStash.Has(e);

            string label = isPlayer ? "Player" : isCamera ? "Camera" : "Entity";
            Vector4 headerColor = isPlayer ? DebugTheme.PlayerColor
                : isCamera ? DebugTheme.CameraEntityColor
                : DebugTheme.ObjectColor;

            ImGui.PushStyleColor(ImGuiCol.Text, headerColor);
            bool open = ImGui.TreeNode($"{label}###{e.GetHashCode()}");
            ImGui.PopStyleColor();

            if (!open) continue;

            // --- Data components with values ---

            if (transformStash.Has(e))
            {
                ref var t = ref transformStash.Get(e);
                ComponentHeader("Transform", DebugTheme.TransformColor);
                Value("Position", $"{t.Position.X:F1}, {t.Position.Y:F1}");
                Value("Velocity", $"{t.Velocity.X:F1}, {t.Velocity.Y:F1}");
                Value("Facing", $"{t.Pivot}");
            }

            if (cameraStash.Has(e))
            {
                ref var cam = ref cameraStash.Get(e);
                ComponentHeader("Camera", DebugTheme.CameraColor);
                Value("Position", $"{cam.Position.X:F1}, {cam.Position.Y:F1}");
                Value("Viewport", $"{cam.Viewport.Width} x {cam.Viewport.Height}");
            }

            if (rectangleCollisionStash.Has(e))
            {
                ref var col = ref rectangleCollisionStash.Get(e);
                ComponentHeader("Collision", DebugTheme.CollisionColor);
                Value("Position", $"{col.Size.X}, {col.Size.Y}");
                Value("Size", $"{col.Size.Width} x {col.Size.Height}");
            }

            if (characterAnimatorStash.Has(e))
            {
                ref var anim = ref characterAnimatorStash.Get(e);
                ComponentHeader("Animator", DebugTheme.AnimatorColor);
                Value("Facing", $"{anim.Facing}");
            }

            if (inventoryStash.Has(e))
            {
                ref var inv = ref inventoryStash.Get(e);
                ComponentHeader("Inventory", DebugTheme.InventoryColor);
                Value("Slots", $"{inv.Slots?.Length ?? 0}");
            }

            // --- Tags as inline badges ---
            bool anyTag = false;

            if (movementAnimationsStash.Has(e)) { TagBadge("Anim", ref anyTag); }
            if (spriteStash.Has(e)) { TagBadge("Sprite", ref anyTag); }
            if (inputMovableStash.Has(e)) { TagBadge("Input", ref anyTag); }
            if (movableStash.Has(e)) { TagBadge("Movable", ref anyTag); }
            if (renderStash.Has(e)) { TagBadge("Render", ref anyTag); }

            if (anyTag)
            {
                ImGui.NewLine();
            }

            ImGui.Separator();
            ImGui.TreePop();
        }

        ImGui.End();
    }

    public void Dispose()
    {
    }

    private static void ComponentHeader(string name, Vector4 color)
    {
        ImGui.Spacing();
        ImGui.PushStyleColor(ImGuiCol.Text, color);
        ImGui.Text(name);
        ImGui.PopStyleColor();
    }

    private static void Value(string label, string value)
    {
        ImGui.TextColored(DebugTheme.TextDim, $"  {label}:");
        ImGui.SameLine();
        ImGui.Text(value);
    }

    private static void TagBadge(string name, ref bool anyBefore)
    {
        if (anyBefore)
        {
            ImGui.SameLine();
        }
        else
        {
            ImGui.Spacing();
            anyBefore = true;
        }

        Vector4 bg = new(DebugTheme.TagColor.X * 0.15f, DebugTheme.TagColor.Y * 0.15f,
            DebugTheme.TagColor.Z * 0.15f, 1f);
        ImGui.PushStyleColor(ImGuiCol.Button, bg);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, bg);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, bg);
        ImGui.PushStyleColor(ImGuiCol.Text, DebugTheme.TagColor);
        ImGui.SmallButton(name);
        ImGui.PopStyleColor(4);
    }
}
