using Microsoft.AspNetCore.SignalR.Client;
using WokerWatachable;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();



await host.RunAsync();