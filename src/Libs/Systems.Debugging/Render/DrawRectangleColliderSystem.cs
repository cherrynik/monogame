using Components.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;

namespace Systems.Debugging.Render;

public class RectangleColliderRenderSystem(
    Scellecs.Morpeh.World world,
    SpriteBatch spriteBatch,
    Texture2D pixel) : IRenderSystem
{
    public Scellecs.Morpeh.World World { get; set; } = world;

    // TODO: Pivot & anchors system
    // TODO: Make circle colliders & the relative system, sloped collision
    public void OnAwake()
    {
    }


    public void OnUpdate(float deltaTime)
    {
        Filter filter = World.Filter
            .With<RectangleColliderComponent>()
            .With<TransformComponent>()
            .Build();

        var camera = World.Filter
            .With<CameraComponent>()
            .Build()
            .First()
            .GetComponent<CameraComponent>();

        foreach (Entity e in filter)
        {
            ref var transform = ref e.GetComponent<TransformComponent>();
            ref var rectCollider = ref e.GetComponent<RectangleColliderComponent>();

            DrawRectangleBorders(camera.WorldToScreen(transform.Position), rectCollider.Size);
        }
    }

    private void DrawRectangleBorders(Vector2 at, Rectangle size)
    {
        var color = Color.White;
        const int thickness = 1;

        DrawTopLine();
        DrawRightLine();
        DrawBottomLine();
        DrawLeftLine();

        return;

        void DrawTopLine() => spriteBatch.Draw(pixel, at, new Rectangle(0, 0, size.Width, thickness), color);

        void DrawRightLine() => spriteBatch.Draw(pixel, new Vector2(at.X + size.Width, at.Y),
            new Rectangle(0, 0, thickness, size.Height), color);

        void DrawBottomLine() => spriteBatch.Draw(pixel, new Vector2(at.X, at.Y + size.Height),
            new Rectangle(0, 0, size.Width, thickness), color);

        void DrawLeftLine() => spriteBatch.Draw(pixel, at, new Rectangle(0, 0, thickness, size.Height), color);
    }

    public void Dispose()
    {
    }
}
