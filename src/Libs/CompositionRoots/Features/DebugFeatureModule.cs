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
    private const int Width = 2;
    private const int Height = 2;


    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton<SystemsList>()
            .RegisterSingleton<EntitiesList>()
            .RegisterSingleton<FrameCounter>()
            .RegisterSingleton<RenderFramesPerSec>()
            .RegisterSingleton<RenderFramesPerSec>()
            .RegisterSingleton(factory =>
            {
                Texture2D colliderPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, Width, Height);
                colliderPixel.SetData(Enumerable.Repeat(Color.LawnGreen, Width * Height).ToArray());
                return new RectangleColliderRenderSystem(factory.GetInstance<World>(),
                    factory.GetInstance<SpriteBatch>(),
                    colliderPixel);
            })
            .RegisterSingleton(factory =>
            {
                Texture2D pivotPixel = new(factory.GetInstance<SpriteBatch>().GraphicsDevice, Width, Height);
                pivotPixel.SetData(Enumerable.Repeat(Color.Khaki, Width * Height).ToArray());

                return new PivotRenderSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(),
                    pivotPixel);
            })
            .RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                factory.GetInstance<SystemsList>(),
                factory.GetInstance<EntitiesList>(),
                factory.GetInstance<FrameCounter>(),
                factory.GetInstance<RenderFramesPerSec>(),
                factory.GetInstance<RectangleColliderRenderSystem>()
                // factory.GetInstance<PivotRenderSystem>()
            ), DiContainerNames.Features.Debug);
    }
}
