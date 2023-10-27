// using System;
// using System.Collections.Generic;
// using Entitas.Components.Data;
// using GameDesktop.Resources.Internal;
// using LightInject;
// using Microsoft.Xna.Framework;
// using MonoGame.Aseprite.Sprites;
// using Services.Math;
//
// namespace GameDesktop.CompositionRoots.Components;
//
// internal class ComponentsCompositionRoot : ICompositionRoot
// {
//     private static readonly string PlayerSpriteSheetPath = System.IO.Path.Join(
//         Environment.GetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory),
//         Resources.SpriteSheet.Player);
//
//     public void Compose(IServiceRegistry serviceRegistry)
//     {
//         RegisterSpriteComponent(serviceRegistry);
//         RegisterMovementAnimationComponent(serviceRegistry);
//         RegisterCameraComponent(serviceRegistry);
//         RegisterTransformComponent(serviceRegistry);
//         RegisterRectangleCollisionComponent(serviceRegistry);
//     }
//
//     private static void RegisterSpriteComponent(IServiceRegistry serviceRegistry)
//     {
//         // todo: put non-game strings in internal resources
//         serviceRegistry.RegisterSingleton(factory =>
//         {
//             var getAnimations =
//                 factory.GetInstance<Func<string, string, Dictionary<Direction, AnimatedSprite>>>("Character");
//             Dictionary<Direction, AnimatedSprite> idle = getAnimations(PlayerSpriteSheetPath, "Idle");
//             var defaultSprite = idle[Direction.Down];
//
//             return new SpriteComponent(defaultSprite);
//         });
//     }
//
//     private static void RegisterMovementAnimationComponent(IServiceRegistry serviceRegistry)
//     {
//         serviceRegistry.RegisterTransient(factory =>
//         {
//             var getAnimations =
//                 factory.GetInstance<Func<string, string, Dictionary<Direction, AnimatedSprite>>>("Character");
//
//             return new MovementAnimationComponent(getAnimations(PlayerSpriteSheetPath, "Idle"),
//                 getAnimations(PlayerSpriteSheetPath, "Walking"));
//         });
//     }
//
//     private static void RegisterCameraComponent(IServiceRegistry serviceRegistry) =>
//         serviceRegistry.RegisterSingleton(_ => new CameraComponent { Size = new Rectangle(0, 0, 640, 480) });
//
//     private static void RegisterTransformComponent(IServiceRegistry serviceRegistry)
//     {
//         serviceRegistry.RegisterSingleton(_ =>
//         {
//             return new TransformComponent { Position = new(316, 116) };
//         }, "Player");
//
//         serviceRegistry.RegisterSingleton(_ =>
//         {
//             return new TransformComponent { Position = new(300, 100) };
//         }, "StaticEntity");
//     }
//
//     private static void RegisterRectangleCollisionComponent(IServiceRegistry serviceRegistry)
//     {
//         serviceRegistry.RegisterTransient(_ => new RectangleCollisionComponent { Size = new Rectangle(0, 0, 8, 8) });
//     }
// }
