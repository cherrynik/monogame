using Components;
using Components.Sprites;
using Entities;
using GameDesktop.Resources;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services.Factories;
using Stateless;
using SpriteSheet = MonoGame.Aseprite.Sprites.SpriteSheet;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly ServiceContainer _container;
    private SpriteBatch _spriteBatch;

    private GameSystems _gameSystems;
    private Contexts _contexts;

    public Game(ServiceContainer container)
    {
        _container = container;
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {

        // TODO: DI with ECS?
        // TODO: Projects management (external in Libs/External?)
        // TODO: Logging with game flags (like LOG_MOVEMENT, etc)?
        // TODO: Error handling
        _contexts = Contexts.sharedInstance;
        _gameSystems = new GameSystems(_contexts, GraphicsDevice);
        _gameSystems.Initialize();
    }

    private void FixedUpdate(GameTime fixedGameTime)
    {
        _gameSystems.FixedExecute(fixedGameTime);
    }

    protected override void Update(GameTime gameTime)
    {
        FixedUpdate(gameTime);

        base.Update(gameTime);
        _gameSystems.Execute(gameTime);

        LateUpdate(gameTime);
    }

    private void LateUpdate(GameTime gameTime)
    {
        _gameSystems.LateExecute(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _gameSystems.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }
}
