using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameDesktop.Resources.Internal;
using LDtk;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Builders;
using Services.Math;

[assembly: CompositionRootType(typeof(GameDesktop.CompositionRoots.FundamentalCompositionRoot))]

namespace GameDesktop.CompositionRoots;

internal class FundamentalCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterLdtk(serviceRegistry);
        RegisterAnimationsFactory(serviceRegistry);
    }

    private static void RegisterLdtk(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ =>
        {
            var fileName = Path.Join(
                Environment.GetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory),
                "Content/TileMaps/Test.ldtk"
            );

            return LDtkFile.FromFile(fileName);
        });
    }

    private static void RegisterAnimationsFactory(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient<AsepriteAnimatedCharactersBuilder>();
        // Warning: binding to <string, T> where T is any type, is dangerous and you should have a different
        // binding off of implementation overloading, if you wanna pass through a string as an arg.
        // So, such resolving won't work either: Func<string, T>, as it'll get it as your string argument is a
        // service name. Thus, I use 3 type args here.
        serviceRegistry.Register<string, string, Dictionary<Sector, AnimatedSprite>>((factory, path, action) =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<GraphicsDeviceManager>().GraphicsDevice;

            return factory.GetInstance<AsepriteAnimatedCharactersBuilder>()
                .LoadSpriteSheet(graphicsDevice, path)
                .CreateAnimations(action)
                .Animations;
        }, "Character");
    }
}
