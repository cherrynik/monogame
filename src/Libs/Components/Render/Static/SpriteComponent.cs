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


namespace Components.Render.Static
{
    using Scellecs.Morpeh;
    using MonoGame.Aseprite.Sprites;

    public struct SpriteComponent : IComponent
    {
        public Sprite Sprite { get; private set; }

        public SpriteComponent(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}
