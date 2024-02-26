using Components.Data;
using Components.Render.Animation;
using Components.Render.Static;
using Components.Tags;
using ImGuiNET;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging;

public class EntitiesList(World world) : IRenderSystem
{
    public World World { set; get; } = world;
    private const float Indentation = 16.0f;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();

        ImGui.Begin("World");

        ImGui.SeparatorText("Entities");
        ImGui.TextWrapped($"Count: {filter.GetLengthSlow()}");
        // if (ImGui.CollapsingHeader("Entities"))

        // ImGui.Indent(Indentation);
        foreach (Entity e in filter)
        {
            // TODO: By flag components I could decide what entity this is and show the proper name
            string entityName = GetEntityName(e);
            if (!ImGui.CollapsingHeader($"{entityName}###{e.ID}")) // ### -> for identical values
            {
                continue;
            }

            ImGui.Indent(Indentation);

            DrawComponentsList(e);

            ImGui.Unindent(Indentation);
        }

        // ImGui.Unindent(Indentation);

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
        Dictionary<Type, string> types = new()
        {
            { typeof(InventoryComponent), "Inventory" },
            { typeof(TransformComponent), "Transform" },
            { typeof(CameraComponent), "Camera" },
            { typeof(RectangleCollisionComponent), "Rectangle Collision" },
            { typeof(CharacterAnimatorComponent), "Character Animator" },
            { typeof(MovementAnimationsComponent), "Movement Animations" },
            { typeof(SpriteComponent), "Sprite" },
            { typeof(InputMovableComponent), "Input Movable" },
            { typeof(MovableComponent), "Movable" },
            { typeof(RenderableComponent), "Renderable" },
        };

        foreach (var type in types.Where(type => e.Has(type.Key)))
        {
            ImGui.TextWrapped(type.Value);
        }
        // TODO: menu for each component to edit the values
    }

    public void Dispose()
    {
    }
}
