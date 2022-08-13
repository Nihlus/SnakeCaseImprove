using BenchmarkDotNet.Attributes;

namespace SnakeCaseImprove;

[MemoryDiagnoser(false)]
public class RandomStringBenchmark
{
    private readonly List<string> _names;
    private readonly string[] _converted = new string[1000];

    private readonly RemoraSnakeCaseNamingPolicy _remora;
    private readonly JsonNetSnakeCaseNamingPolicy _jsonNet;
    private readonly JsonNetOptimizedSbSnakeCaseNamingPolicy _jsonNetOptimizedSb;
    private readonly JsonNetStackallocSnakeCaseNamingPolicy _jsonNetStackalloc;


    public RandomStringBenchmark()
    {
        _names = new List<string>();

        for (var i = 0; i < 1000; i++) _names.Add(RandomString(128, i));


        _remora = new RemoraSnakeCaseNamingPolicy();
        _jsonNet = new JsonNetSnakeCaseNamingPolicy();
        _jsonNetOptimizedSb = new JsonNetOptimizedSbSnakeCaseNamingPolicy();
        _jsonNetStackalloc = new JsonNetStackallocSnakeCaseNamingPolicy();
    }

    public static string RandomString(int length, int seed)
    {
        var random = new Random(seed);
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [Benchmark]
    public string RemoraSingleString()
    {
        return _remora.ConvertName(_names[0]);
    }

    [Benchmark]
    public void Remora1000Strings()
    {
        for (var index = 0; index < _names.Count; index++) _converted[index] = _remora.ConvertName(_names[index]);
    }

    [Benchmark]
    public string JsonNetSingleString()
    {
        return _jsonNet.ConvertName(_names[0]);
    }

    [Benchmark]
    public void JsonNet1000Strings()
    {
        for (var index = 0; index < _names.Count; index++) _converted[index] = _jsonNet.ConvertName(_names[index]);
    }

    [Benchmark]
    public string JsonNetOptimizedSbSingleString()
    {
        return _jsonNetOptimizedSb.ConvertName(_names[0]);
    }

    [Benchmark]
    public void JsonNetOptimizedSb1000Strings()
    {
        for (var index = 0; index < _names.Count; index++)
            _converted[index] = _jsonNetOptimizedSb.ConvertName(_names[index]);
    }

    [Benchmark]
    public string JsonNetStackallocSingleString()
    {
        return _jsonNetStackalloc.ConvertName(_names[0]);
    }

    [Benchmark]
    public void JsonNetStackalloc1000Strings()
    {
        for (var index = 0; index < _names.Count; index++)
            _converted[index] = _jsonNetStackalloc.ConvertName(_names[index]);
    }
}