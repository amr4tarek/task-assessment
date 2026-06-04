using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Background;

public class TaskProcessingBackgroundService(ILogger<TaskProcessingBackgroundService> logger, TaskQueue taskQueue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var taskId = await taskQueue.DequeueAsync(stoppingToken);
            logger.LogInformation("Processing task: {TaskId}", taskId);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}

