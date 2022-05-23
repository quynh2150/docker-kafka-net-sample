//string name = Environment.GetCommandLineArgs()[1];
// builder.Configuration.AddJsonFile("errorcodes.json", false, true);

using KafkaNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerHostedService>();     
        services.AddHostedService<ProducerHostedService>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "";
            options.InstanceName = "";
        });
    })
    .Build()
    .RunAsync();