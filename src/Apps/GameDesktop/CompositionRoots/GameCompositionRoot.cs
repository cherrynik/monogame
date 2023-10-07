using GameDesktop.Resources.Internal;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace GameDesktop.CompositionRoots;

public class GameCompositionRoot : ICompositionRoot
{
    private const bool IsMouseVisible = true;

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterGameServices(serviceRegistry);
    }

    private void RegisterGameServices(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(factory =>
        {
            Game game = new(factory.GetInstance<ILogger>(), factory.GetInstance<IServiceContainer>())
            {
                IsMouseVisible = IsMouseVisible, Content = { RootDirectory = AppVariable.ContentRootDirectory, }
            };

            // Hack. Resolving cycle dependency issue (fundamental architecture)
            // Implicitly adds itself in the game services container.
            game.GraphicsDeviceManager = new GraphicsDeviceManager(game);

            return game;
        });
    }
}
