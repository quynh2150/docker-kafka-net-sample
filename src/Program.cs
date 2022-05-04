//string name = Environment.GetCommandLineArgs()[1];

using KafkaNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerHostedService>();
        services.AddHostedService<ProducerHostedService>();
    })
    .Build()
    .RunAsync();