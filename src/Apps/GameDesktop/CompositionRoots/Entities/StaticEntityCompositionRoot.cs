using System;
using Entities;
using GameDesktop.Resources;
using GameDesktop.Resources.Internal;
using LightInject;

namespace GameDesktop.CompositionRoots.Entities;

public class StaticEntityCompositionRoot : ICompositionRoot
{
    private static readonly string Path = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariable.AppBaseDirectory),
        SpriteSheets.Player);

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient<StaticEntity>();
}
