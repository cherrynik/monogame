using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;
using Services.Math;

namespace Components.Render.Animation;

public struct CharacterAnimatorComponent(Sector facing, AnimatedSprite animation) : IComponent
{
    public Sector Facing = facing;
    public AnimatedSprite Animation = animation;
}
