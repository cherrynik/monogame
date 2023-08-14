using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Math = Service.Math;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<GameDesktop.Game>();

using IHost host = builder.Build();

Main(host.Services);

await host.RunAsync();

static void Main(IServiceProvider hostProvider)
{
    // Test piece of code, using imported shared library
    Math math = new();
    Console.WriteLine($"Service.TestFormula: {math.Multiply(2, 3)}");

    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    using var game = provider.GetRequiredService<GameDesktop.Game>();

    game.Run();
}
