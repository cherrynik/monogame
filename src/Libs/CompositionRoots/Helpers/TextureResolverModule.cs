using Constants;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services.Helpers;

namespace CompositionRoots.Helpers;

public class TextureResolverModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<string, Texture2D>((factory, path) =>
        {
            var fileName = FileResolver.ResolveFromApp(path);

            return Texture2D.FromFile(factory.GetInstance<GraphicsDeviceManager>().GraphicsDevice, fileName);
        }, DINames.Helpers.TextureResolver);
    }
}
