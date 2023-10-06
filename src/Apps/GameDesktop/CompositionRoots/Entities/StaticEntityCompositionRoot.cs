using System;
using Components;
using Components.World;
using Entities;
using Features.Factories;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services.Factories;
using Services.Math;

namespace GameDesktop.CompositionRoots.Entities;

public class StaticEntityCompositionRoot : ICompositionRoot
{
    private static readonly string Path = System.IO.Path.Join(
        Environment.GetEnvironmentVariable(EnvironmentVariables.AppBaseDirectory),
        SpriteSheets.Player);

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterEntity(serviceRegistry);
    }


    private static void RegisterEntity(IServiceRegistry serviceRegistry) =>
        serviceRegistry.RegisterTransient<StaticEntity>();
}
