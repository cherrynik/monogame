using Constants;
using LDtk;
using LightInject;
using Services.Resolvers;

namespace CompositionRoots.Helpers;

public class LdtkModule : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register<LDtkFile>(_ => LdtkResolver.ResolveFromApp(Contents.TileMaps.Test),
            Contents.TileMaps.Test);
    }
}
