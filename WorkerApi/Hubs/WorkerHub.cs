using Microsoft.AspNetCore.SignalR;
using WorkerApi.Data;
using WorkerWatcher;

namespace WorkerApi.Hubs;

public class WorkerHub : Hub<IWorkerContract>
{
    private readonly WorkerManager _workerManager;

    public WorkerHub(WorkerManager workerManager)
    {
        _workerManager = workerManager;
    }
    
    public override Task OnConnectedAsync()
    {
        _workerManager.Add(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _workerManager.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}