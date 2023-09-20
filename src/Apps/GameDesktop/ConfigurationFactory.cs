using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace GameDesktop;

internal static class ConfigurationFactory
{
    private const string AppSettingsName = "appsettings";
    private const string EnvironmentVariableName = "DOTNET_ENVIRONMENT";

    private static string BasePath => Directory.GetParent(AppContext.BaseDirectory)!.FullName;

    private static string CurrentEnvironment =>
        Environment.GetEnvironmentVariable(EnvironmentVariableName) ?? "Production";

    private static readonly string EnvironmentAppSettingsName =
        $"{AppSettingsName}.{CurrentEnvironment}";

    private static string BuildJsonFileName(string fileName) => $"{fileName}.json";

    public static IConfigurationRoot Create() =>
        new ConfigurationBuilder()
            .SetBasePath(BasePath)
            .AddJsonFile(BuildJsonFileName(AppSettingsName), optional: false, reloadOnChange: true)
            .AddJsonFile(BuildJsonFileName(EnvironmentAppSettingsName), optional: true)
            .AddEnvironmentVariables()
            .Build();
}
