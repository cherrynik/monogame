using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;

namespace Components.Render.Animation;

// Facing depends on the velocity,
// So does an animation
public struct CharacterAnimatorComponent : IComponent
{
    public AnimatedSprite Animation;
}
