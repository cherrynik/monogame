using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render.Animation;

public struct CharacterAnimatorComponent : IComponent
{
    public Direction Facing;
    public AnimatedSprite Animation;

    public CharacterAnimatorComponent(Direction facing, AnimatedSprite animation)
    {
        Facing = facing;
        Animation = animation;
    }
}
