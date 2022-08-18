using BenchmarkDotNet.Attributes;

namespace SnakeCaseImprove;

[MemoryDiagnoser(false), RPlotExporter]
public class SnakeCaserBenchmark
{
    private readonly RemoraSnakeCaseNamingPolicy _remora = new();
    private readonly JsonNetSnakeCaseNamingPolicy _jsonNet = new();
    private readonly JsonNetOptimizedSbSnakeCaseNamingPolicy _jsonNetOptimizedSb = new();
    private readonly JsonNetStackallocSnakeCaseNamingPolicy _jsonNetStackalloc = new();
    private readonly VelvetSnakeCaseNamingPolicy _velvet = new();

    [Params("OnceUponATime")]
    public string Value { get; set; }

    [Benchmark(Baseline = true)]
    public string Remora() => _remora.ConvertName(this.Value);

    [Benchmark]
    public string JsonNet() => _jsonNet.ConvertName(this.Value);

    [Benchmark]
    public string JsonNetOptimizedSb() => _jsonNetOptimizedSb.ConvertName(this.Value);

    [Benchmark]
    public string JsonNetStackalloc() => _jsonNetStackalloc.ConvertName(this.Value);

    [Benchmark]
    public string VelvetStackalloc() => _velvet.ConvertName(this.Value);
}
