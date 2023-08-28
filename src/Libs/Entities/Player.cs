using FrontEnd;
using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Services;
using Stateless;

namespace Entities;

public class Player
{
    private readonly IMovement _movement;
    private readonly Texture2D _spriteSheet;
    private readonly IInputScanner _inputScanner;
    private readonly PlayerView _playerView;
    private readonly StateMachine<PlayerState, PlayerTrigger> _stateMachine;
    private Vector2 _position;

    public Player(IMovement movement, IInputScanner inputScanner, PlayerView playerView)
    {
        _movement = movement;
        _inputScanner = inputScanner;
        _playerView = playerView;

        _stateMachine = new StateMachine<PlayerState, PlayerTrigger>(PlayerState.Idle);
        _stateMachine.Configure(PlayerState.Idle)
            .OnActivate(() =>
            {
                // face in the last direction or right by default
                _playerView.Apply(Vector2.Zero);
            })
            .OnEntry(() =>
            {
                // face in the last direction or right by default
                _playerView.Apply(Vector2.Zero);
            })
            .Ignore(PlayerTrigger.Stop)
            .Permit(PlayerTrigger.SpeedUp, PlayerState.Walking);

        _stateMachine.Configure(PlayerState.Walking).OnEntry(() =>
            {
                // face in the direction you walk
                // play animation of walking
                Vector2 direction = _inputScanner.GetDirection();
                _playerView.Apply(direction);
            })
            .Ignore(PlayerTrigger.SpeedUp)
            // .Permit(PlayerTrigger.SpeedUp, PlayerState.Running)
            .Permit(PlayerTrigger.Stop, PlayerState.Idle);

        // _stateMachine.Configure(PlayerState.Running).OnEntry(() =>
        //     {
        //         // face in the direction you walk
        //         // play animation of running
        //     })
        //     .Permit(PlayerTrigger.Stop, PlayerState.Idle)
        //     .Permit(PlayerTrigger.SlowDown, PlayerState.Walking);

        _stateMachine.Activate();
    }

    // I prefer having update in a single place, but fo' now sum like diz
    public void Update(GameTime gameTime)
    {
        Vector2 direction = _inputScanner.GetDirection();
        
        if (direction.Equals(Vector2.Zero))
        {
            _stateMachine.Fire(PlayerTrigger.Stop);
            return;
        }
        
        _stateMachine.Fire(PlayerTrigger.SpeedUp);

        _position = _movement.Move(_position, direction);

        _playerView.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // spriteBatch.Begin();
        // spriteBatch.Draw(_spriteSheet, _position, Color.White);
        // spriteBatch.End();
        _playerView.Draw(spriteBatch, _position);
    }
}

internal enum PlayerState
{
    Idle,
    Walking,
    Running
}

internal enum PlayerTrigger
{
    Stop,
    SpeedUp,
    SlowDown
}
