using MonoGame.Aseprite.Sprites;
using Scellecs.Morpeh;

namespace Components.Render.Static;

public readonly struct SpriteComponent(Sprite sprite) : IComponent
{
    public Sprite Sprite { get; } = sprite;
}
