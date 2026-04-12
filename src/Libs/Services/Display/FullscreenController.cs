using Microsoft.Xna.Framework;

namespace Services.Display;

public sealed class FullscreenController
{
    private readonly GraphicsDeviceManager? _graphicsDeviceManager;

    public FullscreenController(GraphicsDeviceManager? graphicsDeviceManager)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
    }

    public void Toggle()
    {
        if (_graphicsDeviceManager is null)
        {
            return;
        }

        _graphicsDeviceManager.IsFullScreen = !_graphicsDeviceManager.IsFullScreen;
        _graphicsDeviceManager.ApplyChanges();
    }
}
