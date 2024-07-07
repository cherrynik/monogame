using Constants;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Implementations.Camera;
using Services.Resolvers;
using Systems.Render;

namespace CompositionRoots.Features;

public class RenderFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory => new TilesRenderSystem(factory.GetInstance<World>(),
                factory.GetInstance<SpriteBatch>(),
                LdtkResolver.ResolveFromApp(Contents.TileMaps.Test)))
            .RegisterSingleton(factory => new EntitiesRenderSystem(factory.GetInstance<World>(),
                factory.GetInstance<SpriteBatch>()))
            .RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
                factory.GetInstance<SystemsEngine>(),
                factory.GetInstance<TilesRenderSystem>(),
                factory.GetInstance<EntitiesRenderSystem>()
            ), DiContainerNames.Features.Render);
    }
}
