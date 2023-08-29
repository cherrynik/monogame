using Services;
using Stateless;

namespace FrontEnd;

// public class Temp
// {
//     private readonly StateMachine<PlayerState, PlayerTrigger> _stateMachine;
//
//     public Temp(StateMachine<PlayerState, PlayerTrigger> stateMachine)
//     {
//         _stateMachine = stateMachine;
//
//         ConfigureStateMachine();
//     }
//
//     private void ConfigureStateMachine()
//     {
//         _stateMachine
//             .Configure(PlayerState.Idle)
//             .OnEntry(() =>
//             {
//                 RadDir radDir = MathUtils.Rad8DirYFlipped(_direction);
//                 _animatedSprite = _idleAnimatedSprites[radDir];
//                 _animatedSprite.SetFrame(0);
//             })
//             .Ignore(PlayerTrigger.Stop)
//             .Permit(PlayerTrigger.SpeedUp, PlayerState.Walking);
//
//         _stateMachine
//             .Configure(PlayerState.Walking)
//             .OnEntry(() =>
//             {
//                 RadDir radDir = MathUtils.Rad8DirYFlipped(_direction);
//
//                 var walkingAnimatedSprite = _walkingAnimatedSprites[radDir];
//                 if (_animatedSprite.Name != walkingAnimatedSprite.Name ||
//                     _animatedSprite.FlipHorizontally != walkingAnimatedSprite.FlipHorizontally) // Temp hack
//                 {
//                     _animatedSprite = walkingAnimatedSprite;
//                 }
//             })
//             .PermitReentry(PlayerTrigger.SpeedUp)
//             .Permit(PlayerTrigger.Stop, PlayerState.Idle);
//
//         _stateMachine.Activate();
//     }
// }

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

