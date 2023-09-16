using LightInject;
using Microsoft.Xna.Framework;
using Serilog.Core;

namespace GameDesktop;

public class GameCompositionRoot : ICompositionRoot
{
    private const string ContentRootDirectory = "Content";
    private const bool IsMouseVisible = true;

    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(_ => Contexts.sharedInstance);

        serviceRegistry.Register(factory =>
        {
            Game game = new(
                factory.GetInstance<Logger>(),
                factory.GetInstance<Contexts>()
            ) { IsMouseVisible = IsMouseVisible, Content = { RootDirectory = ContentRootDirectory, } };

            // Hack. Resolving cycle dependency issue (fundamental architecture)
            // Implicitly adds itself in the game services container.
            new GraphicsDeviceManager(game);

            return game;
        });
    }
}
