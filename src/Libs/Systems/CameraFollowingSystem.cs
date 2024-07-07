using Components.Data;
using Implementations.Camera;
using Microsoft.Xna.Framework;
using Scellecs.Morpeh;

namespace Systems;

// issue: https://gamedev.stackexchange.com/questions/46963/how-to-avoid-texture-bleeding-in-a-texture-atlas
// scaling, viewport & matrix: https://www.youtube.com/watch?v=BVSSQKlYipo&ab_channel=AristurtleDev
public class CameraFollowingSystem(
    World world,
    GraphicsDeviceManager graphicsDeviceManager,
    ICameraFollowing cameraImpl)
    : IFixedSystem
{
    public World World { get; set; } = world;

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        // TODO: Components in the range of visibility (world grid system?)
        Filter filter = World.Filter.With<CameraComponent>().Build();

        if (filter.IsEmpty()) return;

        Entity e = filter.First();

        ref var target = ref e.GetComponent<TransformComponent>();
        ref var camera = ref e.GetComponent<CameraComponent>();

        // https://community.monogame.net/t/jittering-with-lerping-camera/15899/9
        camera.Viewport = graphicsDeviceManager.GraphicsDevice.Viewport;

        camera.Position = cameraImpl.Move(from: camera.Position,
            to: camera.GetCenteredPosition(target.Position, new(10_000, 10_000)),
            step: .2f);
    }

    public void Dispose()
    {
    }
}
