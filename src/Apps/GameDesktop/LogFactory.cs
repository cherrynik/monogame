using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace GameDesktop;

internal static class LogFactory
{
    public static Logger Create(IConfiguration configuration) =>
        new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
}
