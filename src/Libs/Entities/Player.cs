using Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;

namespace Entities;

public enum PlayerState
{
    Idle,
    Walking,
    Running
}

public enum PlayerTrigger
{
    Stop,
    SpeedUp,
    SlowDown
}

public class Player
{
    private readonly IMovement _movement;
    private readonly IInputScanner _inputScanner;
    private readonly StateMachine<PlayerState, PlayerTrigger> _stateMachine;
    private readonly Dictionary<RadDir, AnimatedSprite> _walkingAnimatedSprites;
    private readonly Dictionary<RadDir, AnimatedSprite> _idleAnimatedSprites;
    private AnimatedSprite _animatedSprite;

    private Vector2 _position;
    private Vector2 _direction;

    // TODO: Move state machine & its config outta here?
    public Player(IMovement movement,
        IInputScanner inputScanner,
        StateMachine<PlayerState, PlayerTrigger> stateMachine,
        Dictionary<RadDir, AnimatedSprite> walkingAnimatedSprites,
        Dictionary<RadDir, AnimatedSprite> idleAnimatedSprites)
    {
        _movement = movement;
        _inputScanner = inputScanner;

        _stateMachine = stateMachine;

        _walkingAnimatedSprites = walkingAnimatedSprites;
        _idleAnimatedSprites = idleAnimatedSprites;

        _animatedSprite = _idleAnimatedSprites[RadDir.Right];

        _stateMachine
            .Configure(PlayerState.Idle)
            .OnEntry(() =>
            {
                RadDir radDir = MathUtils.Rad8DirYFlipped(_direction);
                _animatedSprite = _idleAnimatedSprites[radDir];
                _animatedSprite.SetFrame(0);
            })
            .Ignore(PlayerTrigger.Stop)
            .Permit(PlayerTrigger.SpeedUp, PlayerState.Walking);

        _stateMachine
            .Configure(PlayerState.Walking)
            .OnEntry(() =>
            {
                RadDir radDir = MathUtils.Rad8DirYFlipped(_direction);

                var walkingAnimatedSprite = _walkingAnimatedSprites[radDir];
                if (_animatedSprite.Name != walkingAnimatedSprite.Name ||
                    _animatedSprite.FlipHorizontally != walkingAnimatedSprite.FlipHorizontally) // Temp hack
                {
                    _animatedSprite = walkingAnimatedSprite;
                }
            })
            .PermitReentry(PlayerTrigger.SpeedUp)
            .Permit(PlayerTrigger.Stop, PlayerState.Idle);

        _stateMachine.Activate();
    }

    public void Update(GameTime gameTime)
    {
        Vector2 direction = _inputScanner.GetDirection();

        if (direction.Equals(Vector2.Zero))
        {
            _stateMachine.Fire(PlayerTrigger.Stop);
        }
        else
        {
            _stateMachine.Fire(PlayerTrigger.SpeedUp);

            _direction = direction;

            _position = _movement.Move(_position, _direction);
        }

        _animatedSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _animatedSprite.Draw(spriteBatch, _position);
        spriteBatch.End();
    }
}
