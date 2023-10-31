using System.Collections.Generic;
using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Factories;
using Services.Math;

namespace GameDesktop.CompositionRoots;

internal class FundamentalCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterAnimationsFactory(serviceRegistry);
        serviceRegistry.RegisterSingleton(typeof(AbstractFactory<>));
    }

    private static void RegisterAnimationsFactory(IServiceRegistry serviceRegistry)
    {
        // Warning: binding to <string, T> where T is any type, is dangerous and you should have a different
        // binding off of implementation overloading, if you wanna pass through a string as an arg.
        // So, such resolving won't work either: Func<string, T>, as it'll get it as your string argument is a
        // service name. Thus, I use 3 type args here.
        serviceRegistry.Register<string, string, Dictionary<Direction, AnimatedSprite>>((factory, path, action) =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet = AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, path);

            return AnimatedCharactersFactory.CreateAnimations(spriteSheet, action);
        }, "Character");
    }
}
