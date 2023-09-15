using LightInject;
using Serilog;
using Serilog.Events;

namespace GameDesktop;

public class LoggingCompositionRoot : ICompositionRoot
{
    private const LogEventLevel MinimumLevel = LogEventLevel.Verbose;
    private const string FilePath = "logs/log-.txt";

    private const string OutputTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj} {NewLine}{Exception}";

    private const bool DoSentryDebug = false;
    private const LogEventLevel SentryMinimumBreadcrumbLevel = LogEventLevel.Debug;

    // TODO: Put it in env
    private const string SentryDsn =
        "https://ff3f6fec4457d740ab0a98c123e77086@o4505883399487488.ingest.sentry.io/4505883401388032";

    public void Compose(IServiceRegistry serviceRegistry)
    {
        serviceRegistry.Register(_ => new LoggerConfiguration()
                .MinimumLevel.Is(MinimumLevel)
                .Enrich.FromLogContext()
                .WriteTo.File(FilePath,
                    outputTemplate: OutputTemplate,
                    rollingInterval: RollingInterval.Day)
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .WriteTo.Sentry(options =>
                {
                    // Debug and higher are stored as breadcrumbs (default is Information)
                    options.MinimumBreadcrumbLevel = SentryMinimumBreadcrumbLevel;
                    options.Debug = DoSentryDebug;
                    options.Dsn = SentryDsn;
                })
                .CreateLogger(),
            new PerContainerLifetime());
    }
}
