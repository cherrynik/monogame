using Components.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Implementations.Camera;
using Vector2 = System.Numerics.Vector2;

namespace Systems.Render;

// issue: https://gamedev.stackexchange.com/questions/46963/how-to-avoid-texture-bleeding-in-a-texture-atlas
// scaling, viewport & matrix: https://www.youtube.com/watch?v=BVSSQKlYipo&ab_channel=AristurtleDev
public class CameraFollowingSystem(World world, GraphicsDeviceManager graphicsDeviceManager, ICameraFollowing camera)
    : IRenderSystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        // TODO: Components in the range of visibility (world grid system?)
        // Filter filter = World.Filter.With<TransformComponent>().Build();
        // IEnumerable<Entity> entities = SortEntitiesByYPosition(filter);
        //
        // foreach (Entity e in entities)
        // {
        //     _camera.Render(e);
        // }

        Filter filter = World.Filter.With<CameraComponent>().Build();

        if (filter.IsEmpty())
            return;

        Entity e = filter.First();

        ref var transform = ref e.GetComponent<TransformComponent>();
        ref var entityCamera = ref e.GetComponent<CameraComponent>();

        // https://community.monogame.net/t/jittering-with-lerping-camera/15899/9
        // entityCamera.Position = Vector2.Lerp(entityCamera.Position,
        //     GetCenteredPosition(
        //         new(0, 0, graphicsDeviceManager.GraphicsDevice.Viewport.Width,
        //             graphicsDeviceManager.GraphicsDevice.Viewport.Height), off: transform.Position),
        //     .1f);
        var to = GetCenteredPosition(
            new(0, 0, graphicsDeviceManager.GraphicsDevice.Viewport.Width,
                graphicsDeviceManager.GraphicsDevice.Viewport.Height), off: transform.Position);
        entityCamera.Position = camera.Move(from: entityCamera.Position, to);
    }

    private static Vector2 GetCenteredPosition(Viewport viewport, Vector2 off)
    {
        var cameraX = off.X - (float)viewport.Width / 2;
        var cameraY = off.Y - (float)viewport.Height / 2;

        cameraX = MathHelper.Clamp(cameraX, 0, 50_000 - viewport.Width);
        cameraY = MathHelper.Clamp(cameraY, 0, 50_000 - viewport.Height);

        return new Vector2(cameraX, cameraY);
    }

    public void Dispose()
    {
    }
}
