using LightInject;

namespace GameDesktop;

public class LoggingCompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry) =>
        serviceRegistry.Register(_ => LogFactory.Create(), new PerContainerLifetime());
}
