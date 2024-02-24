// using Entitas.Components.Tags;
// using MonoGame.Aseprite.Sprites;
// using Scellecs.Morpeh;
//
// namespace Entitas.Components.Data
// {
//     public class SpriteComponent : RenderComponent
//     {
//         public Sprite? Sprite;
//
//         public SpriteComponent()
//         {
//         }
//
//         public SpriteComponent(Sprite sprite)
//         {
//             Sprite = sprite;
//         }
//     }
// }


using Scellecs.Morpeh;
using MonoGame.Aseprite.Sprites;

namespace Components.Render.Static;

public struct SpriteComponent(Sprite sprite) : IComponent
{
    public Sprite Sprite { get; private set; } = sprite;
}
