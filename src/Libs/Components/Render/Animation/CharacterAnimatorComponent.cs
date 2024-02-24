using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render.Animation;

public struct CharacterAnimatorComponent(Direction facing, AnimatedSprite animation) : IComponent
{
    public Direction Facing = facing;
    public AnimatedSprite Animation = animation;
}
