using System;
using System.IO;
using GameDesktop.Resources;
using Microsoft.Extensions.Configuration;

namespace GameDesktop;

internal static class ConfigurationFactory
{
    private static string BasePath => Directory.GetParent(AppContext.BaseDirectory)!.FullName;

    private static string InEnvironment =>
        Environment.GetEnvironmentVariable(EnvironmentVariables.DotNetEnvironment) ?? "Production";

    public static IConfigurationRoot Create() =>
        new ConfigurationBuilder()
            .SetBasePath(BasePath)
            .AddJsonFile($"{AppVariables.SettingsName}.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{AppVariables.SettingsName}.{InEnvironment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
}
