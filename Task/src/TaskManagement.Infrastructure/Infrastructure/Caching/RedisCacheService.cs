using System.Text.Json;
using StackExchange.Redis;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Caching;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : ICacheService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _database.StringGetAsync(key);
        if (!value.HasValue)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, payload, expiration);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await _database.KeyDeleteAsync(key);
}

