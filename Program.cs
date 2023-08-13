﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<monogame.Game>();

using IHost host = builder.Build();

Main(host.Services);

await host.RunAsync();

static void Main(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    using var game = provider.GetRequiredService<monogame.Game>();

    game.Run();
}
