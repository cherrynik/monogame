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
