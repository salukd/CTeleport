using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NSubstitute;
using StackExchange.Redis;
using WeatherForecast.Infrastructure.Services.Cache;


namespace WeatherForecast.Infrastructure.UnitTests.Services.Cache;

public class CacheServiceTests
{
    private readonly IDatabase _mockDatabase;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        var mockRedis = Substitute.For<IConnectionMultiplexer>();
        _mockDatabase = Substitute.For<IDatabase>();
        mockRedis.GetDatabase().Returns(_mockDatabase);
        _cacheService = new CacheService(mockRedis);
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
    }


    [Fact]
    public async Task GetOrCreateAsync_WhenCacheHit_ShouldReturnCachedValue()
    {
        // Arrange
        var key = "testKey";
        var cachedValue = new TestData { Id = 1, Name = "Test" };
        var serializedValue = JsonSerializer.Serialize(cachedValue, GetJsonSerializerOptions());
        _mockDatabase.StringGetAsync(key).Returns(serializedValue);

        // Act
        Func<CancellationToken, Task<TestData>> factory = _ => Task.FromResult(new TestData { Id = 2, Name = "New Test" });
        var result = await _cacheService.GetOrCreateAsync<TestData>(
            key,
            factory,
            null,
            CancellationToken.None
        );

        // Assert
        result.Should().BeEquivalentTo(cachedValue);
        await _mockDatabase.DidNotReceive().StringSetAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue>(), Arg.Any<TimeSpan?>());
    }

    [Fact]
    public async Task GetOrCreateAsync_WhenCacheMiss_ShouldCallFactoryAndCacheResult()
    {
        // Arrange
        var key = "testKey";
        var factoryValue = new TestData { Id = 2, Name = "New Test" };
        _mockDatabase.StringGetAsync(key).Returns(RedisValue.Null);

        // Act
        var result = await _cacheService.GetOrCreateAsync(key, _ => Task.FromResult(factoryValue));

        // Assert
        result.Should().BeEquivalentTo(factoryValue);
        await _mockDatabase.Received(1).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Is<TimeSpan>(t => t == TimeSpan.FromMinutes(5))
        );
    }

    [Fact]
    public async Task GetOrCreateAsync_WithCustomExpiration_ShouldUseProvidedExpiration()
    {
        // Arrange
        var key = "testKey";
        var factoryValue = new TestData { Id = 3, Name = "Custom Expiration" };
        var customExpiration = TimeSpan.FromHours(1);
        _mockDatabase.StringGetAsync(key).Returns(RedisValue.Null);

        // Act
        await _cacheService.GetOrCreateAsync(key, _ => Task.FromResult(factoryValue), customExpiration);

        // Assert
        await _mockDatabase.Received(1).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Is<TimeSpan>(t => t == customExpiration)
        );
    }

    [Fact]
    public async Task GetOrCreateAsync_ShouldPassCancellationTokenToFactory()
    {
        // Arrange
        var key = "testKey";
        var cts = new CancellationTokenSource();
        _mockDatabase.StringGetAsync(key).Returns(RedisValue.Null);

        // Act
        await _cacheService.GetOrCreateAsync(key, ct =>
        {
            ct.Should().Be(cts.Token);
            return Task.FromResult(new TestData());
        }, cancellationToken: cts.Token);

        // Assert
        // The assertion is done inside the factory method
    }

    [Fact]
    public async Task GetOrCreateAsync_WithComplexObject_ShouldSerializeAndDeserializeCorrectly()
    {
        // Arrange
        var key = "complexKey";
        var complexObject = new ComplexTestData
        {
            Id = 4,
            Name = "Complex",
            SubData = new TestData { Id = 5, Name = "Sub" }
        };
        _mockDatabase.StringGetAsync(key).Returns(RedisValue.Null);

        // Act
        var result = await _cacheService.GetOrCreateAsync(key, _ => Task.FromResult(complexObject));

        // Assert
        result.Should().BeEquivalentTo(complexObject);
        await _mockDatabase.Received(1).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Any<TimeSpan>()
        );
    }

    private class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class ComplexTestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestData SubData { get; set; }
    }
}