using System.Threading.Channels;

namespace TaskManagement.Infrastructure.Background;

public class TaskQueue
{
    private readonly Channel<Guid> _channel = Channel.CreateUnbounded<Guid>();

    public ValueTask QueueAsync(Guid taskId, CancellationToken cancellationToken = default)
        => _channel.Writer.WriteAsync(taskId, cancellationToken);

    public ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken = default)
        => _channel.Reader.ReadAsync(cancellationToken);
}

