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
    private Filter _transformFilter = default!;

    public EntitiesList(World world)
    {
        World = world;
    }

    public void OnAwake()
    {
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

        if (ImGui.CollapsingHeader("Entities"))
        {
            ImGui.Indent(Indentation);
            foreach (Entity e in _transformFilter)
            {
                // TODO: By flag components I could decide what entity this is and show the proper name
                if (!ImGui.CollapsingHeader($"Entity###{e.GetHashCode()}")) // ### -> keep IDs stable per frame for duplicate labels
                {
                    continue;
                }

                ImGui.Indent(Indentation);

                // TODO: collect all the components' names automatically
                // TODO: use List<Type> types -> Has(typeof(T)) (with underlying Has<T>());
                if (inventoryStash.Has(e)) ImGui.TextWrapped(nameof(InventoryComponent));
                if (transformStash.Has(e)) ImGui.TextWrapped(nameof(TransformComponent));
                if (cameraStash.Has(e)) ImGui.TextWrapped(nameof(CameraComponent));
                if (rectangleCollisionStash.Has(e)) ImGui.TextWrapped(nameof(RectangleCollisionComponent));
                if (characterAnimatorStash.Has(e)) ImGui.TextWrapped(nameof(CharacterAnimatorComponent));
                if (movementAnimationsStash.Has(e)) ImGui.TextWrapped(nameof(MovementAnimationsComponent));
                if (spriteStash.Has(e)) ImGui.TextWrapped(nameof(SpriteComponent));
                if (inputMovableStash.Has(e)) ImGui.TextWrapped(nameof(InputMovableComponent));
                if (movableStash.Has(e)) ImGui.TextWrapped(nameof(MovableComponent));
                if (renderStash.Has(e)) ImGui.TextWrapped(nameof(RenderComponent));
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
