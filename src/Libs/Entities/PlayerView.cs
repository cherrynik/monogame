using Entitas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Stateless;

namespace Entities;

// TODO: Extract an interface?
public class PlayerView
{
    private const RadDir DefaultFacing = RadDir.Down;

    private readonly StateMachine<PlayerState, PlayerTrigger> _stateMachine;

    private readonly StateMachine<PlayerState, PlayerTrigger>.TriggerWithParameters<Vector2>
        _moveWithParameters;

    private readonly Dictionary<RadDir, AnimatedSprite> _idleAnimations;
    private readonly Dictionary<RadDir, AnimatedSprite> _walkingAnimations;

    private Vector2 _lastDirection;
    private AnimatedSprite _playingAnimation;

    public PlayerView(StateMachine<PlayerState, PlayerTrigger> stateMachine,
        Dictionary<RadDir, AnimatedSprite> idleAnimations,
        Dictionary<RadDir, AnimatedSprite> walkingAnimations)
    {
        _stateMachine = stateMachine;
        _idleAnimations = idleAnimations;
        _walkingAnimations = walkingAnimations;
        _playingAnimation = _idleAnimations[DefaultFacing];

        _moveWithParameters = _stateMachine.SetTriggerParameters<Vector2>(PlayerTrigger.Move);

        ConfigureStateMachine();

        _stateMachine.Activate();
    }

    public void Update(GameTime gameTime, Vector2 direction)
    {
        if (direction.Equals(Vector2.Zero))
        {
            _stateMachine.Fire(PlayerTrigger.Stop);
        }
        else
        {
            _stateMachine.Fire(_moveWithParameters, direction);
        }

        _playingAnimation.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 at)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _playingAnimation.Draw(spriteBatch, at);
        spriteBatch.End();
    }

    private void ConfigureStateMachine()
    {
        _stateMachine
            .Configure(PlayerState.Idle)
            .OnEntry(StopFacingTrace)
            .Ignore(PlayerTrigger.Stop)
            .Permit(PlayerTrigger.Move, PlayerState.Moving);

        _stateMachine
            .Configure(PlayerState.Moving)
            .OnEntryFrom(_moveWithParameters, MoveFacingTrace)
            .PermitReentry(PlayerTrigger.Move)
            .Permit(PlayerTrigger.Stop, PlayerState.Idle);
    }

    private void StopFacingTrace()
    {
        RadDir radDir = MathUtils.Rad8DirYFlipped(_lastDirection);
        _playingAnimation = _idleAnimations[radDir];
        _playingAnimation.SetFrame(0);
    }

    private void MoveFacingTrace(Vector2 direction)
    {
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        AnimatedSprite walkingAnimation = _walkingAnimations[radDir];

        if (AreDifferentSprites(_playingAnimation, walkingAnimation))
        {
            _playingAnimation = walkingAnimation;
        }

        _lastDirection = direction;
    }

    private static bool AreDifferentSprites(Sprite left, Sprite right) =>
        left.Name != right.Name ||
        left.FlipHorizontally != right.FlipHorizontally;
}

public enum PlayerState
{
    Idle,
    Moving,
}

public enum PlayerTrigger
{
    Stop,
    Move,
}
