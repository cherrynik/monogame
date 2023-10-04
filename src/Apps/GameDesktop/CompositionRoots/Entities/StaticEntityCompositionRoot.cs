using System;
using Components;
using Components.World;
using Features.Factories;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Factories;
using Services.Math;

namespace GameDesktop.CompositionRoots;

public class StaticEntityCompositionRoot : ICompositionRoot
{
    private static readonly string Path = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariables.AppBaseDirectory),
        SpriteSheets.Player);

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterComponents(serviceRegistry);
        RegisterEntity(serviceRegistry);
    }

    private static void RegisterComponents(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterTransient(_ => new TransformComponent
        {
            Position = new Vector2(new Random().Next(0, 100), new Random().Next(0, 100))
        });

        serviceRegistry.Register(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            // TODO: Refactor into func<>
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, Path);

            AnimatedSprite animatedSprite =
                AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle")[Direction.Down];
            return new SpriteComponent(animatedSprite);
        });
    }

    private static void RegisterEntity(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            var contexts = factory.GetInstance<Contexts>();
            var transform = factory.GetInstance<TransformComponent>();
            var sprite = factory.GetInstance<SpriteComponent>();

            return new StaticEntity(contexts, transform, sprite);
        });
    }
}
