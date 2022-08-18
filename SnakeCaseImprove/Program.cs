using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using SnakeCaseImprove;

var config = DefaultConfig.Instance;
config.AddExporter(CsvMeasurementsExporter.Default);
config.AddExporter(RPlotExporter.Default);

BenchmarkRunner.Run<SnakeCaserBenchmark>(config);
