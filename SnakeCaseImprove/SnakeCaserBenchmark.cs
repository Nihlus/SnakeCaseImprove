using BenchmarkDotNet.Attributes;

namespace SnakeCaseImprove;

[MemoryDiagnoser(false), RPlotExporter]
public class SnakeCaserBenchmark
{
    private readonly RemoraSnakeCaseNamingPolicy _remora = new();
    private readonly JsonNetStackallocSnakeCaseNamingPolicy _jsonNetStackalloc = new();
    private readonly JaxSnakeCaseNamingPolicy _jax = new();

    [Benchmark(Baseline = true)]
    [Arguments("OnceUponATime")]
    public string Remora(string value) => _remora.ConvertName(value);

    [Benchmark]
    [Arguments("OnceUponATime")]
    public string JsonNetStackalloc(string value) => _jsonNetStackalloc.ConvertName(value);

    [Benchmark]
    [Arguments("OnceUponATime")]
    public string JaxStackalloc(string value) => _jax.ConvertName(value);
}
