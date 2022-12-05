using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR.Client;
using WorkerWatcher;

namespace WokerWatachable;

public class Worker : IWorkerContract, IHostedService
{
    private readonly ILogger<Worker> _logger;
    private HubConnection _connection;
    

    private bool _runningStatus = true;
    private string _lastStatus = String.Empty;
    private int _runningTimeInSeconds = 123123123;
    private string WorkerName = $"Generic Worker = {Guid.NewGuid().ToString()}";

    private CancellationToken _cancellationToken;
    
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(RunJob(cancellationToken), RunHubConnection());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task RunJob(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time} Worker Name : {WorkerName}", DateTimeOffset.Now,WorkerName);
            await Task.Delay(1000, stoppingToken);
        }
    }

    public async Task RunHubConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7238/WorkerHub")
            .WithAutomaticReconnect()
            .Build();

        _connection.On(nameof(ShutDown), ShutDown);
        _connection.On(nameof(RunningTime), RunningTime);
        _connection.On(nameof(GetLastResult), GetLastResult);
        _connection.On(nameof(GetWorkerName), GetWorkerName);
        _connection.On(nameof(GetRunningStatus), GetRunningStatus);

        await _connection.StartAsync();
    }
    
    public Task<bool> GetRunningStatus()
    {
        return Task.FromResult(_runningStatus);
    }

    public Task<int> RunningTime()
    {
        return Task.FromResult(_runningTimeInSeconds);
    }

    public Task<string> GetWorkerName()
    {
        return Task.FromResult(WorkerName);
    }

    public Task<string> GetLastResult()
    {
        return Task.FromResult(_lastStatus);
    }

    public Task ShutDown()
    {
        _logger.LogInformation("Shutdown");
        Environment.Exit(0);
        return Task.CompletedTask;
    }


}