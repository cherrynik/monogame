using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using Services;
using Services.Math;
using Stateless;

namespace Components.Sprites;

public class MovementAnimatedSprites : IComponent
{
    private const RadDir DefaultFacing = RadDir.Down;

    public StateMachine<PlayerState, PlayerTrigger> StateMachine;

    public StateMachine<PlayerState, PlayerTrigger>.TriggerWithParameters<Vector2>
        MoveWithParameters;

    private readonly Dictionary<RadDir, AnimatedSprite> _idleAnimations;
    private readonly Dictionary<RadDir, AnimatedSprite> _walkingAnimations;
    private readonly SpriteBatch _spriteBatch;

    private Vector2 _lastDirection;

    public AnimatedSprite PlayingAnimation;

    public MovementAnimatedSprites()
    {
    }

    public MovementAnimatedSprites(StateMachine<PlayerState, PlayerTrigger> stateMachine,
        Dictionary<RadDir, AnimatedSprite> idleAnimations,
        Dictionary<RadDir, AnimatedSprite> walkingAnimations, SpriteBatch spriteBatch)
    {
        StateMachine = stateMachine;
        MoveWithParameters = StateMachine.SetTriggerParameters<Vector2>(PlayerTrigger.Move);

        _idleAnimations = idleAnimations;
        _walkingAnimations = walkingAnimations;
        _spriteBatch = spriteBatch;
        PlayingAnimation = _idleAnimations[DefaultFacing];

        ConfigureStateMachine();

        StateMachine.Activate();
    }

    private void ConfigureStateMachine()
    {
        StateMachine
            .Configure(PlayerState.Idle)
            .OnEntry(StopFacingTrace)
            .Ignore(PlayerTrigger.Stop)
            .Permit(PlayerTrigger.Move, PlayerState.Moving);

        StateMachine
            .Configure(PlayerState.Moving)
            .OnEntryFrom(MoveWithParameters, MoveFacingTrace)
            .PermitReentry(PlayerTrigger.Move)
            .Permit(PlayerTrigger.Stop, PlayerState.Idle);
    }

    private void StopFacingTrace()
    {
        RadDir radDir = MathUtils.Rad8DirYFlipped(_lastDirection);
        PlayingAnimation = _idleAnimations[radDir];
        PlayingAnimation.SetFrame(0);
    }

    private void MoveFacingTrace(Vector2 direction)
    {
        RadDir radDir = MathUtils.Rad8DirYFlipped(direction);
        AnimatedSprite walkingAnimation = _walkingAnimations[radDir];

        if (AreDifferentSprites(PlayingAnimation, walkingAnimation))
        {
            PlayingAnimation = walkingAnimation;
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
