using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR;
using WorkerApi.Hubs;
using WorkerWatcher;

namespace WorkerApi.Data;

public class WorkerManager
{
    private readonly IHubContext<WorkerHub, IWorkerContract> _hubContext;
    
    public ObservableCollection<(string, IWorkerContract)> Workers { get; } = new();
    
    public WorkerManager(IHubContext<WorkerHub, IWorkerContract> hubContext)
    {
        this._hubContext = hubContext;
    }

    public void Add(string connectionId)
    {
        lock (Workers)
        {
            Workers.Add((connectionId,_hubContext.Clients.Client(connectionId)));
        }
    }
    
    public void Remove(string connectionId)
    {
        lock (Workers)
        {
            var worker = Workers.FirstOrDefault(item => item.Item1 == connectionId);
            Workers.Remove(worker);
        }
    }
}