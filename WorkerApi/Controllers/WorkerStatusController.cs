using Microsoft.AspNetCore.Mvc;
using WorkerApi.Data;

namespace WorkerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkerStatusController : ControllerBase
{
    private readonly WorkerManager _manager;

    public WorkerStatusController(WorkerManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet]
    public IActionResult GetWorkerStatus()
    {
        var x = _manager.Workers.Select(x => x.Item2.GetWorkerName());

        var result = Task.WhenAll(x);

        return Ok(result.Result);
    }
    
    [HttpDelete]
    public IActionResult CloseAllWorkers()
    {
        var x = _manager.Workers.Select(x => x.Item2.ShutDown());

        var result = Task.WhenAll(x);

        return Ok();
    }
}