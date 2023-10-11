using Components.Tags;
using MonoGame.Aseprite.Sprites;

namespace Components.Data;

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
