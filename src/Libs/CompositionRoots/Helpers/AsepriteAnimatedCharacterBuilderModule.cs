using Constants;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Builders;
using Services.Implementations.Math;

namespace CompositionRoots.Helpers;

public class AsepriteAnimatedCharacterBuilderModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient<AsepriteAnimatedCharactersBuilder>();

        // Warning: binding to <string, T> where T is any type, is dangerous, and you should have a different
        // binding off of implementation overloading, if you want to pass through a string as an arg.
        // So, such resolving won't work either: Func<string, T>, as it'll get it as your string argument is a
        // service name. Thus, I use 3 type args here.
        serviceRegistry.Register<string, string, Dictionary<Sector, AnimatedSprite>>((factory, path, action) =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<GraphicsDeviceManager>().GraphicsDevice;

            return factory.GetInstance<AsepriteAnimatedCharactersBuilder>()
                .LoadSpriteSheet(graphicsDevice, path)
                .CreateAnimations(action)
                .Animations;
        }, DINames.Helpers.AsepriteAnimatedCharacterBuilder);
    }
}
