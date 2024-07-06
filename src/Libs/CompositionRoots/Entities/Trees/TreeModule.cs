using Components.Data;
using Components.Render.Static;
using Constants;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;

namespace CompositionRoots.Entities.Trees;

public class TreeModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var texture = factory.GetInstance<string, Texture2D>(Contents.Textures.Main);
            // TODO: Automatically get texture & its rect from a tile set (by aseprite?)
            var sprite = new Sprite(DINames.Tree, new TextureRegion(DINames.Tree, texture, new(144, 0, 48, 96)));

            return new SpriteComponent(sprite, factory.GetInstance<TransformComponent>());
        }, DINames.Tree);
    }
}
