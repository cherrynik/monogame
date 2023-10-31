using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render.Animation;

// Facing depends on the velocity,
// So does an animation
public struct CharacterAnimatorComponent : IComponent
{
    public Direction Facing;
    public AnimatedSprite Animation;
}
