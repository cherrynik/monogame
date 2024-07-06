using Constants;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Systems.Debugging.Diagnostics;
using Systems.Debugging.Render;
using Systems.Debugging.World;

namespace CompositionRoots.Features;

public class DebugFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
        {
            const int w = 2, h = 2;
            Texture2D pivotPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, w, h);
            pivotPixel.SetData(Enumerable.Repeat(Color.Khaki, w * h).ToArray());

            Texture2D colliderPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, w, h);
            colliderPixel.SetData(Enumerable.Repeat(Color.LawnGreen, w * h).ToArray());

            return new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                new SystemsList(factory.GetInstance<World>(),
                    factory.GetInstance<SystemsEngine>()),
                new EntitiesList(factory.GetInstance<World>()),
                new FrameCounter(factory.GetInstance<World>()),
                new RenderFramesPerSec(factory.GetInstance<World>(), factory.GetInstance<IServiceFactory>()),
                new RectangleColliderRenderSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(),
                    colliderPixel)
                // new PivotRenderSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(), pivotPixel)
            );
        }, DiContainerNames.Features.Debug);
    }
}
