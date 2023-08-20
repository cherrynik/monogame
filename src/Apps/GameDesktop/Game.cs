using Entities;
using LightInject;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Services;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private const string ContentRootDirectory = "Content";
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player _player;
    private Texture2D _texture;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = ContentRootDirectory;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _texture = Content.Load<Texture2D>("player");

        // Really, XNA or MonoGame have created a puzzle by passing "this" into GraphicsDeviceManager,
        // creating an unsolvable circular dependency problem, by doing so.
        // So, for now, ole tha entry-point logic will be here.
        ServiceContainer container = new();
        container.Register<IMovement, SimpleMovement>();
        container.Register<IInputScanner, KeyboardScanner>();
        container.Register(factory =>
            new Player(factory.GetInstance<IMovement>(), _texture, factory.GetInstance<IInputScanner>()));

        _player = container.GetInstance<Player>();
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _player.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _player.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
