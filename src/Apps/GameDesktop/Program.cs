using System;
using LightInject;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Sentry;
using Game = GameDesktop.Game;

SentrySdk.Init(options =>
{
    // A Sentry Data Source Name (DSN) is required.
    // See https://docs.sentry.io/product/sentry-basics/dsn-explainer/
    // You can set it in the SENTRY_DSN environment variable, or you can set it in code here.

    // TODO: Set it in the SENTRY_DSN environment variable
    // options.Dsn = "https://examplePublicKey@o0.ingest.sentry.io/0";
    options.Dsn =
        "https://ff3f6fec4457d740ab0a98c123e77086@o4505883399487488.ingest.sentry.io/4505883401388032";

    // When debug is enabled, the Sentry client will emit detailed debugging information to the console.
    // This might be helpful, or might interfere with the normal operation of your application.
    // We enable it here for demonstration purposes when first trying Sentry.
    // You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
    options.Debug = true;

    // This option is recommended. It enables Sentry's "Release Health" feature.
    options.AutoSessionTracking = true;

    // Enabling this option is recommended for client applications only. It ensures all threads use the same global scope.
    options.IsGlobalModeEnabled = false;

    // This option will enable Sentry's tracing features. You still need to start transactions and spans.
    options.EnableTracing = true;

    // Example sample rate for your transactions: captures 10% of transactions
    options.TracesSampleRate = 0.1;
});

try
{
// Game Container Start-Up config
    using ServiceContainer container = new();

    // container.Register(_ => loggerFactory.AddSentry());
    container.Register(_ => LoggerFactory.Create(builder =>
        builder
            .AddConsole()
            .AddSentry()
    ).CreateLogger<Game>());

    ILogger<Game> logger = container.GetInstance<ILogger<Game>>();

    container.Register(factory =>
    {
        Game game = new(logger) { IsMouseVisible = true, Content = { RootDirectory = "Content" } };

        // Hack. Resolving cycle dependency issue (fundamental architecture)
        // Implicitly adds itself in the game services container.
        new GraphicsDeviceManager(game);

        return game;
    });

// Maybe not needed,
// as we have the GraphicsDevice field in the Game class
// container.Register(factory =>
//     factory.GetInstance<Game>()
//         .Services
//         .GetService<GraphicsDeviceManager>());


    logger.LogInformation("Dependencies Injected");
    using Game game = container.GetInstance<Game>();
    game.Run();
}
catch (Exception e)
{
    SentrySdk.CaptureException(e);
}
