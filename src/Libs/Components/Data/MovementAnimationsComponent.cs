using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Data;

public struct MovementAnimationsComponent(
    Dictionary<Sector, AnimatedSprite> idleAnimations,
    Dictionary<Sector, AnimatedSprite> walkingAnimations)
    : IComponent
{
    public readonly Dictionary<Sector, AnimatedSprite> IdleAnimations = idleAnimations;
    public readonly Dictionary<Sector, AnimatedSprite> WalkingAnimations = walkingAnimations;
}
