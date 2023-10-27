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

namespace Components.Render.Animation
{
    using Scellecs.Morpeh;
    using MonoGame.Aseprite.Sprites;
    using Services.Math;

    public struct MovementAnimationsComponent : IComponent
    {
        public readonly Dictionary<Direction, AnimatedSprite> IdleAnimations;
        public readonly Dictionary<Direction, AnimatedSprite> WalkingAnimations;

        public MovementAnimationsComponent(Dictionary<Direction, AnimatedSprite> idleAnimations,
            Dictionary<Direction, AnimatedSprite> walkingAnimations)
        {
            IdleAnimations = idleAnimations;
            WalkingAnimations = walkingAnimations;
        }
    }

    // CharacterAnimatorComponent
}
