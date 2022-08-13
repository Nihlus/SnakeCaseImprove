# SnakeCaseImprove
Benchmark of different snake case conversion methods

|                         Method |           Mean |       Error |      StdDev |   Allocated |
|------------------------------- |---------------:|------------:|------------:|------------:|
|             RemoraSingleString |     3,446.0 ns |     6.13 ns |     5.73 ns |     1,864 B |
|              Remora1000Strings | 4,788,162.1 ns | 5,290.28 ns | 4,948.54 ns | 1,861,700 B |
|            JsonNetSingleString |     1,227.3 ns |     5.29 ns |     4.95 ns |     1,240 B |
|             JsonNet1000Strings | 1,493,626.6 ns | 2,042.66 ns | 1,910.71 ns | 1,237,841 B |
| JsonNetOptimizedSbSingleString |     1,032.4 ns |     9.90 ns |     9.26 ns |       824 B |
|  JsonNetOptimizedSb1000Strings | 1,246,180.5 ns |   582.19 ns |   486.15 ns |   821,842 B |
|  JsonNetStackallocSingleString |       813.2 ns |     9.39 ns |     8.78 ns |       368 B |
|   JsonNetStackalloc1000Strings | 1,125,884.4 ns |   403.12 ns |   357.36 ns |   365,841 B |

Note: In rare and odd inputs, stackalloc method will fail and produce out of bounds esception

In most cases optimal buffer is 1.5x of input size, but rare odd inputs can produce more than that, requiring reallocating to 2x input size

It is currently difficult to realloc span in safe C#

[JSON.NET](https://github.com/JamesNK/Newtonsoft.Json)

[Remora](https://github.com/Remora/Remora.Rest)
