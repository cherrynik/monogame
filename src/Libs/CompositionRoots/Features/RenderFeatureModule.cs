using Constants;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Extended;
using Services.Helpers;
using Systems.Render;

namespace CompositionRoots.Features;

public class RenderFeatureModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory => new Feature(factory.GetInstance<World>(),
            factory.GetInstance<SystemsEngine>(),
            new TilesRenderingSystem(factory.GetInstance<World>(), factory.GetInstance<SpriteBatch>(),
                LdtkResolver.ResolveFromApp(Contents.TileMaps.Test)),
            new RenderCharacterMovementAnimationSystem(factory.GetInstance<World>(),
                factory.GetInstance<SpriteBatch>())), DINames.Features.Render);
    }
}
