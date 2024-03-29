﻿// using System;
// using Entitas;
// using Features;
// using GameDesktop.Resources.Internal;
// using LightInject;
// using Serilog;
// using Services;
// using Services.Movement;
// using Systems;
//
// namespace GameDesktop.CompositionRoots.Features;
//
// internal class MovementFeatureCompositionRoot : ICompositionRoot
// {
//     private static readonly IMatcher<GameEntity>[] CollisionMatchers =
//     {
//         GameMatcher.RectangleCollision, GameMatcher.Transform
//     };
//
//     private static readonly IMatcher<GameEntity>[] MovableMatchers = { GameMatcher.Transform, GameMatcher.Movable };
//
//     private static readonly IMatcher<GameEntity>[] AnimatedMovableMatchers =
//     {
//         GameMatcher.Movable, GameMatcher.MovementAnimation
//     };
//
//     public void Compose(IServiceRegistry serviceRegistry)
//     {
//         RegisterImpl(serviceRegistry);
//         RegisterSystems(serviceRegistry);
//         RegisterFeature(serviceRegistry);
//     }
//
//     private static void RegisterImpl(IServiceRegistry serviceRegistry) =>
//         serviceRegistry.RegisterSingleton<IMovement, SimpleMovement>();
//
//     private static void RegisterSystems(IServiceRegistry serviceRegistry)
//     {
//         serviceRegistry.RegisterSingleton(factory =>
//         {
//             var getGroup = factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
//             IGroup<GameEntity> group = getGroup(CollisionMatchers);
//
//             var logger = factory.GetInstance<ILogger>();
//
//             return new CollisionSystem(group, logger);
//         });
//
//         serviceRegistry.RegisterSingleton(factory =>
//         {
//             var movement = factory.GetInstance<IMovement>();
//
//             var getGroup = factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
//             IGroup<GameEntity> group = getGroup(MovableMatchers);
//
//             var logger = factory.GetInstance<ILogger>();
//
//             return new MovementSystem(movement, group, logger);
//         });
//
//         serviceRegistry.RegisterSingleton(factory =>
//         {
//             var getGroup = factory.GetInstance<Func<IMatcher<GameEntity>[], IGroup<GameEntity>>>(Matcher.AllOf);
//             IGroup<GameEntity> group = getGroup(AnimatedMovableMatchers);
//
//             var logger = factory.GetInstance<ILogger>();
//
//             return new AnimatedMovementSystem(group, logger);
//         });
//     }
//
//     private static void RegisterFeature(IServiceRegistry serviceRegistry) =>
//         serviceRegistry.RegisterSingleton<MovementFeature>();
// }
