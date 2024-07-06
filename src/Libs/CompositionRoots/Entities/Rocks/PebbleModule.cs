using Components.Data;
using Components.Render.Static;
using Constants;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;

namespace CompositionRoots.Entities.Rocks;

public class PebbleModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            var texture = factory.GetInstance<string, Texture2D>(Contents.Textures.Main);
            // TODO: Automatically get texture & its rect from a tile set (by aseprite?)
            var sprite = new Sprite(DINames.Pebble, new TextureRegion(DINames.Pebble, texture, new(208, 48, 16, 16)));

            return new SpriteComponent(sprite, factory.GetInstance<TransformComponent>());
        }, DINames.Pebble);
    }
}
