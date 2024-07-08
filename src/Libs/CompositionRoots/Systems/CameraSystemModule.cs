using Components.Data;
using Constants;
using Implementations.Camera;
using LightInject;
using Microsoft.Xna.Framework.Graphics;
using Systems;
using Systems.Render;

namespace CompositionRoots.Systems;

public class CameraSystemModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(factory =>
                new CameraComponent(factory.GetInstance<Viewport>(DiContainerNames.Camera)))
            .RegisterSingleton(_ => new Viewport(0, 0, 801, 480), DiContainerNames.Camera)
            .RegisterSingleton<ICamera, Camera>()
            .RegisterSingleton<CameraFollowingSystem>();
    }
}
