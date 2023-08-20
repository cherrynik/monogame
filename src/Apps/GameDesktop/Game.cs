using System;
using System.Numerics;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GameDesktop;

public class Game : Microsoft.Xna.Framework.Game
{
    private const string ContentRootDirectory = "Content";
    private readonly IMovement _movement;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Vector2 _position;
    private Texture2D _texture;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        _movement = new SimpleMovement();
        _position = new Vector2(0, 0);
        Content.RootDirectory = ContentRootDirectory;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture = Content.Load<Texture2D>("player");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // TODO: Add your update logic here
        KeyboardState keyboardState = Keyboard.GetState();
        float horizontalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Right)) -
                              Convert.ToSingle(keyboardState.IsKeyDown(Keys.Left));

        float verticalDir = Convert.ToSingle(keyboardState.IsKeyDown(Keys.Down)) -
                            Convert.ToSingle(keyboardState.IsKeyDown(Keys.Up));

        if (horizontalDir != 0 || verticalDir != 0)
        {
            _position = _movement.Move(_position, new Vector2 { X = horizontalDir, Y = verticalDir, });
            Console.WriteLine(_position.ToString());
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        // Rectangle
        _spriteBatch.Begin();
        Rectangle rect = new Rectangle((int)_position.X, (int)_position.Y, 200, 150);
        Color rectColor = Color.Red;
        Texture2D pixelTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { rectColor });
        _spriteBatch.Draw(pixelTexture, rect, rectColor);
        _spriteBatch.End();
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, new Vector2(0, 0), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
