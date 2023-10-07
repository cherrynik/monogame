using Entitas;
using MonoGame.Aseprite.Sprites;

namespace Components;

public class SpriteComponent : DrawableComponent
{
    public Sprite? Sprite;

    public SpriteComponent()
    {
    }

    public SpriteComponent(Sprite sprite)
    {
        Sprite = sprite;
    }
}
