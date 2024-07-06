using System;
using System.IO;
using Constants;
using Microsoft.Extensions.Configuration;

namespace GameDesktop.Builders;

internal static class ConfigurationBuilder
{
    private static string BasePath => Directory.GetParent(AppContext.BaseDirectory)!.FullName;

    private static string InEnvironment =>
        Environment.GetEnvironmentVariable(EnvironmentNames.DotNetEnvironment) ?? "Production";

    public static IConfigurationRoot Create() =>
        new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(BasePath)
            .AddJsonFile($"{AppVariables.SettingsFileName}.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{AppVariables.SettingsFileName}.{InEnvironment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
}
