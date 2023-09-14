using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = GameDesktop.Game;

// Game Container Start-Up config
using ServiceContainer container = new();

container.Register(_ =>
{
    Game game = new(container) { IsMouseVisible = true, Content = { RootDirectory = "Content" } };

    // Hack. Resolving cycle dependency issue (fundamental architecture)
    // Implicitly adds itself in the game services container.
    new GraphicsDeviceManager(game);

    return game;
});

// Maybe not needed,
// as we have the GraphicsDevice field in the Game class
// container.Register(factory =>
//     factory.GetInstance<Game>()
//         .Services
//         .GetService<GraphicsDeviceManager>());

using Game game = container.GetInstance<Game>();
game.Run();
