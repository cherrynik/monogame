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
    private const float Indentation = 16.0f;

    public EntitiesList(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter.With<TransformComponent>().Build();

        ImGui.Begin("World");

        if (ImGui.CollapsingHeader("Entities"))
        {
            ImGui.Indent(Indentation);
            foreach (Entity e in filter)
            {
                // TODO: By flag components I could decide what entity this is and show the proper name
                if (!ImGui.CollapsingHeader($"Entity###{e.ID}")) // ### -> for identical values
                {
                    continue;
                }

                ImGui.Indent(Indentation);

                // TODO: collect all the components' names automatically
                // TODO: use List<Type> types -> Has(typeof(T)) (with underlying Has<T>());
                if (e.Has<InventoryComponent>()) ImGui.TextWrapped(nameof(InventoryComponent));
                if (e.Has<TransformComponent>()) ImGui.TextWrapped(nameof(TransformComponent));
                if (e.Has<CameraComponent>()) ImGui.TextWrapped(nameof(CameraComponent));
                if (e.Has<RectangleCollisionComponent>()) ImGui.TextWrapped(nameof(RectangleCollisionComponent));
                if (e.Has<CharacterAnimatorComponent>()) ImGui.TextWrapped(nameof(CharacterAnimatorComponent));
                if (e.Has<MovementAnimationsComponent>()) ImGui.TextWrapped(nameof(MovementAnimationsComponent));
                if (e.Has<SpriteComponent>()) ImGui.TextWrapped(nameof(SpriteComponent));
                if (e.Has<InputMovableComponent>()) ImGui.TextWrapped(nameof(InputMovableComponent));
                if (e.Has<MovableComponent>()) ImGui.TextWrapped(nameof(MovableComponent));
                if (e.Has<RenderComponent>()) ImGui.TextWrapped(nameof(RenderComponent));
                // TODO: menu for each component to edit the values

                ImGui.Unindent(Indentation);
            }

            ImGui.Unindent(Indentation);
        }

        ImGui.End();
    }

    public void Dispose()
    {
    }
}
