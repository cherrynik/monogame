using System;
using Components;
using Components.World;
using Entitas;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Factories;
using Services.Math;

namespace GameDesktop.CompositionRoots;

public class FundamentalCompositionRoot : ICompositionRoot
{
    private static readonly string Path = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariables.AppBaseDirectory),
        SpriteSheets.Player);

    private const string AllOf = "AllOf";
    private const string AnyOf = "AnyOf";

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterContexts(serviceRegistry);
        RegisterAllOfMatcher(serviceRegistry);
        RegisterComponents(serviceRegistry);
    }

    private static void RegisterContexts(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterInstance(Contexts.sharedInstance);

    private static void RegisterAllOfMatcher(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<IMatcher<GameEntity>[], IGroup<GameEntity>>((factory, matchers) =>
        {
            IAllOfMatcher<GameEntity> groupMatcher = GameMatcher.AllOf(matchers);
            var contexts = factory.GetInstance<Contexts>();
            return contexts.game.GetGroup(groupMatcher);
        }, AllOf);
    }

    private static void RegisterComponents(IServiceRegistry serviceRegistry)
    {
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

        serviceRegistry.RegisterTransient(_ => new TransformComponent
        {
            Position = new Vector2(new Random().Next(0, 100), new Random().Next(0, 100))
        });

        serviceRegistry.RegisterSingleton(_ => new CameraComponent { Size = new Rectangle(0, 0, 640, 480) });

        serviceRegistry.RegisterTransient(factory =>
        {
            GraphicsDevice graphicsDevice = factory.GetInstance<SpriteBatch>().GraphicsDevice;
            SpriteSheet spriteSheet =
                AnimatedCharactersFactory.LoadSpriteSheet(graphicsDevice, Path);

            var idleAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Idle");
            var walkingAnimations = AnimatedCharactersFactory.CreateAnimations(spriteSheet, "Walking");

            return new MovementAnimationComponent(idleAnimations, walkingAnimations);
        });
    }
}
