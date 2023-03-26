using StackExchange.Redis;

namespace CacheImpl;

public class RedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisService(string url)
    {
        _connectionMultiplexer=ConnectionMultiplexer.Connect(url);
    }

    public IDatabase GetDb(int db = 0) => _connectionMultiplexer.GetDatabase(db);
}