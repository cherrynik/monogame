using Features;
using LightInject;
using Microsoft.Xna.Framework;
using Serilog;

namespace GameDesktop;

public class GameCompositionRoot : ICompositionRoot
{
    private const string ContentRootDirectory = "Content";
    private const bool IsMouseVisible = true;

    public void Compose(IServiceRegistry serviceRegistry)
    {
        RegisterGameServices(serviceRegistry);
    }

    private void RegisterGameServices(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(_ => Contexts.sharedInstance);

        serviceRegistry.RegisterFrom<RootFeatureCompositionRoot>();

        serviceRegistry.Register(factory =>
        {
            Game game = new(
                factory.GetInstance<ILogger>(),
                factory.GetInstance<Contexts>(),
                factory.GetInstance<IServiceContainer>()
            ) { IsMouseVisible = IsMouseVisible, Content = { RootDirectory = ContentRootDirectory, } };

            // Hack. Resolving cycle dependency issue (fundamental architecture)
            // Implicitly adds itself in the game services container.
            new GraphicsDeviceManager(game);

            return game;
        });
    }
}
