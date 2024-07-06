using Components.Data;
using Constants;
using LightInject;
using Microsoft.Xna.Framework.Graphics;

namespace CompositionRoots.Entities;

public class CameraModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.RegisterSingleton(_ => new Viewport(0, 0, 801, 480), DINames.Camera);
        serviceRegistry.RegisterSingleton(factory =>
            new CameraComponent(factory.GetInstance<Viewport>(DINames.Camera)));
    }
}
