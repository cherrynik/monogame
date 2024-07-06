using Scellecs.Morpeh;
using MonoGame.Aseprite.Sprites;
using Services.Implementations.Math;

namespace Components.Render.Animation;

public struct MovementAnimationsComponent(
    Dictionary<Sector, AnimatedSprite> idleAnimations,
    Dictionary<Sector, AnimatedSprite> walkingAnimations)
    : IComponent
{
    public readonly Dictionary<Sector, AnimatedSprite> IdleAnimations = idleAnimations;
    public readonly Dictionary<Sector, AnimatedSprite> WalkingAnimations = walkingAnimations;
}
