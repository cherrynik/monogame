using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Scellecs.Morpeh.Extended.Extensions;

namespace Systems.Debugging.World;

public class EntitiesList(Scellecs.Morpeh.World world) : IRenderSystem
{
    public Scellecs.Morpeh.World World { set; get; } = world;
    private const float Indentation = 16.0f;

    static readonly Dictionary<Type, string> Types = new()
    {
        { typeof(InventoryComponent), "Inventory" },
        { typeof(TransformComponent), "Transform" },
        { typeof(CameraComponent), "Camera" },
        { typeof(RectangleColliderComponent), "Rectangle Collision" },
        { typeof(CharacterAnimatorComponent), "Character Animator" },
        { typeof(MovementAnimationsComponent), "Movement Animations" },
        { typeof(SpriteComponent), "Sprite" },
        { typeof(InputMovableComponent), "Input Movable" },
        { typeof(MovableComponent), "Movable" },
        { typeof(RenderableComponent), "Renderable" },
    };

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();

        ImGui.Begin("World");

        if (!ImGui.TreeNode($"Entities ({filter.GetLengthSlow()})"))
        {
            return;
        }


        foreach (Entity e in filter)
        {
            if (!ImGui.TreeNode($"{GetEntityName(e)}##{e.ID}")) // ## -> for identical values
            {
                continue;
            }

            DrawComponentsList(e);

            ImGui.TreePop();
        }

        ImGui.TreePop();

        ImGui.End();
    }

    private static string GetEntityName(Entity entity)
    {
        ref NameComponent nameComponent = ref entity.GetComponent<NameComponent>();
        return string.IsNullOrWhiteSpace(nameComponent.Name) ? "Entity" : nameComponent.Name;
    }

    private static void DrawComponentsList(Entity e)
    {
        // TODO: collect all the components' names automatically

        foreach (var (key, value) in Types.Where(type => e.Has(type.Key)))
        {
            // FIXME: button to open closable window (now it's screwed, the window is closed when unfocused)
            if (ImGui.Button(value)) ImGui.OpenPopup(value);

            if (!ImGui.IsPopupOpen(value))
            {
                continue;
            }

            ImGui.BeginPopup(value, ImGuiWindowFlags.ChildMenu);

            DrawComponentEditor(key, e);

            ImGui.End();
        }
        // TODO: menu for each component to edit the values
    }

    // TODO: refactor this?
    private static void DrawComponentEditor(Type component, Entity e)
    {
        if (component == typeof(TransformComponent))
        {
            ref TransformComponent transformComponent = ref e.GetComponent<TransformComponent>();

            ImGui.SliderFloat2("Position", ref transformComponent.Position, 0, 300);
        }
        else if (component == typeof(InventoryComponent))
        {
            // TODO: table with dropdown selectable items of the range of ItemsTable & ItemIds enum at the slots
            ref InventoryComponent inventoryComponent = ref e.GetComponent<InventoryComponent>();
            ImGui.SeparatorText("Slots");
            for (var i = 0; i < inventoryComponent.Slots.Length; i++)
            {
                var slot = inventoryComponent.Slots[i];
                ImGui.TextWrapped($"{i + 1}: {slot.GetInfo().Name}");
            }
        }
    }

    public void Dispose()
    {
    }
}
