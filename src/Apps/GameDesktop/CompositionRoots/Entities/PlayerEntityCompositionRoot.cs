﻿using Components.Data;
using Components.Render.Animation;
using Components.Tags;
using Entities.Factories.Characters;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

internal class PlayerEntityCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }

    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient(factory => new PlayerEntityFactory(
            new NameComponent("Player"), // factory.GetInstance<NameComponent>("Player"),
            factory.GetInstance<InputMovableComponent>(),
            factory.GetInstance<MovableComponent>(),
            factory.GetInstance<TransformComponent>("PlayerEntity"),
            factory.GetInstance<CameraComponent>(),
            factory.GetInstance<RectangleColliderComponent>("PlayerEntity"),
            factory.GetInstance<MovementAnimationsComponent>(),
            factory.GetInstance<CharacterAnimatorComponent>("PlayerEntity"),
            factory.GetInstance<InventoryComponent>()));
}
