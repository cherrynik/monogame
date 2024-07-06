using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace GameDesktop.Builders;

internal static class LoggerBuilder
{
    public static Logger Create(IConfiguration configuration) =>
        new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
}
