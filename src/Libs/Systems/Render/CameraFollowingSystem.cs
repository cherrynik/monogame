using System.Numerics;
using Components.Data;
using Components.Events.Movement;
using Components.Tags;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;

namespace Systems.Render;

public class CameraFollowingSystem : ILateSystem
{
    private bool _isInitialized;
    private bool _hasViewportSnapshot;
    private int _lastViewportWidth;
    private int _lastViewportHeight;
    private readonly GraphicsDevice? _graphicsDevice;
    public World World { get; set; }

    public CameraFollowingSystem(World world, GraphicsDevice? graphicsDevice = null)
    {
        World = world;
        _graphicsDevice = graphicsDevice;
    }

    public void OnAwake()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        var transformStash = World.GetStash<TransformComponent>();
        var cameraStash = World.GetStash<CameraComponent>();
        Viewport viewport = default;
        bool hasRuntimeViewport = _graphicsDevice is not null;
        bool viewportChanged = false;
        if (hasRuntimeViewport)
        {
            viewport = _graphicsDevice!.Viewport;
            viewportChanged = !_hasViewportSnapshot ||
                              viewport.Width != _lastViewportWidth ||
                              viewport.Height != _lastViewportHeight;
        }

        var cameraFilter = !_isInitialized || viewportChanged
            ? World.Filter.With<CameraComponent>().With<TransformComponent>().Build()
            : World.Filter.With<CameraComponent>().With<TransformComponent>().With<TransformMovedEvent>().Build();

        foreach (Entity e in cameraFilter)
        {
            ref var transform = ref transformStash.Get(e);
            ref var camera = ref cameraStash.Get(e);

            if (hasRuntimeViewport)
            {
                camera.Viewport = viewport;
                _lastViewportWidth = viewport.Width;
                _lastViewportHeight = viewport.Height;
                _hasViewportSnapshot = true;
            }

            camera.Position = GetCenteredPosition(camera.Viewport, off: transform.Position);
            _isInitialized = true;
            break;
        }
    }

    private static Vector2 GetCenteredPosition(Viewport viewport, Vector2 off) => new(
        off.X - viewport.Width / 2,
        off.Y - viewport.Height / 2);

    public void Dispose()
    {
    }
}
