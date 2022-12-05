namespace WorkerWatcher;

public interface IWorkerContract
{
    Task<bool> GetRunningStatus();
    Task<int> RunningTime();
    Task<string> GetWorkerName();
    Task<string> GetLastResult();
    Task ShutDown();
}