// using Entitas.Components.Data;
// using Entitas.Entities;
// using LightInject;
//
// namespace GameDesktop.CompositionRoots.Entities;
//
// internal class StaticEntityCompositionRoot : ICompositionRoot
// {
//     public void Compose(IServiceRegistry serviceRegistry)
//     {
//         RegisterEntity(serviceRegistry);
//     }
//
//
//     private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
//         serviceRegistry.RegisterTransient(factory => new StaticEntity(factory.GetInstance<Contexts>(),
//             factory.GetInstance<TransformComponent>("StaticEntity"),
//             factory.GetInstance<SpriteComponent>(),
//             factory.GetInstance<RectangleCollisionComponent>()));
// }
