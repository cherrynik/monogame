// namespace Entitas.Components.Data
// {
//     public class MovementAnimationComponent : RenderComponent
//     {
//         private const Direction DefaultFacing = Direction.Down;
//
//         public AnimatedSprite? PlayingAnimation;
//         public Dictionary<Direction, AnimatedSprite>? IdleAnimations;
//         public Dictionary<Direction, AnimatedSprite>? WalkingAnimations;
//
//         public Vector2 FacingDirection = Vector2.UnitY;
//         public bool HasStopped;
//
//         public MovementAnimationComponent()
//         {
//         }
//
//         public MovementAnimationComponent(Dictionary<Direction, AnimatedSprite> idleAnimations,
//             Dictionary<Direction, AnimatedSprite> walkingAnimations)
//         {
//             IdleAnimations = idleAnimations;
//             WalkingAnimations = walkingAnimations;
//
//             PlayingAnimation = idleAnimations[DefaultFacing];
//         }
//     }
// }

using Scellecs.Morpeh;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace Components.Render.Animation;

public struct MovementAnimationsComponent(
    Dictionary<Direction, AnimatedSprite> idleAnimations,
    Dictionary<Direction, AnimatedSprite> walkingAnimations)
    : IComponent
{
    public readonly Dictionary<Direction, AnimatedSprite> IdleAnimations = idleAnimations;
    public readonly Dictionary<Direction, AnimatedSprite> WalkingAnimations = walkingAnimations;
}

// CharacterAnimatorComponent
