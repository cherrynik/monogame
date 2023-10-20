using Components.Tags;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Sprites;
using Services.Math;

namespace Components.Data;

public class MovementAnimationComponent : DrawableComponent
{
    private const Direction DefaultFacing = Direction.Down;

    public AnimatedSprite? PlayingAnimation;
    public Dictionary<Direction, AnimatedSprite>? IdleAnimations;
    public Dictionary<Direction, AnimatedSprite>? WalkingAnimations;

    public Vector2 FacingDirection = Vector2.UnitY;
    public bool HasStopped;

    public MovementAnimationComponent()
    {
    }

    public MovementAnimationComponent(Dictionary<Direction, AnimatedSprite> idleAnimations,
        Dictionary<Direction, AnimatedSprite> walkingAnimations)
    {
        IdleAnimations = idleAnimations;
        WalkingAnimations = walkingAnimations;

        PlayingAnimation = idleAnimations[DefaultFacing];
    }
}
