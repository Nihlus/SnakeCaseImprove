using System.Buffers;
using System.Text.Json;

namespace SnakeCaseImprove;

public class VelvetSnakeCaseNamingPolicy : JsonNamingPolicy
{
    private readonly bool _upperCase;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VelvetSnakeCaseNamingPolicy" /> class.
    /// </summary>
    /// <param name="upperCase">Whether the converted names should be in all upper case.</param>
    public VelvetSnakeCaseNamingPolicy(bool upperCase = false)
    {
        _upperCase = upperCase;
    }

    /// <inheritdoc />
    public override string ConvertName(string name) => _upperCase
        ? name.Snake().ToUpperInvariant()
        : name.Snake();
}

public static class FastSnakeCaser
{
    private const byte _stackAllocationCap = 128;

    public static string Snake(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var outputSize = input.Length * 2; // worst case, it's typically between 1.3 and 1.7
        return outputSize <= _stackAllocationCap
            ? SnakeStackAlloc(input, outputSize)
            : SnakeHeapAlloc(input, outputSize);
    }

    private static string SnakeStackAlloc(string input, int outputSize)
    {
        Span<char> output = stackalloc char[outputSize];
        var length = SnakeCore(input, output);
        return new(output[..length]);
    }

    private static string SnakeHeapAlloc(string input, int outputSize)
    {
        var output = ArrayPool<char>.Shared.Rent(outputSize);
        try
        {
            var length = SnakeCore(input, output);
            return new string(output[..length]);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(output);
        }
    }

    private static int SnakeCore(ReadOnlySpan<char> input, Span<char> output)
    {
        output[0] = char.ToLowerInvariant(input[0]);
        var outputIndex = 1;

        var upperCount = char.IsUpper(input[0]) ? 1 : 0;
        var isPreviousUpper = char.IsUpper(input[0]);
        var isPreviousLetter = char.IsLetter(input[0]);

        for (var index = 1; index < input.Length; index++)
        {
            var c = input[index];
            var isCurrentLetter = char.IsLetter(c);
            if (!isCurrentLetter)
            {
                output[outputIndex++] = c;

                upperCount = 0;
                isPreviousLetter = false;
                isPreviousUpper = false;

                continue;
            }

            var isCurrentUpper = char.IsUpper(c);
            if (isCurrentUpper)
            {
                upperCount++;
            }

            if (!isPreviousLetter)
            {
                output[outputIndex++] = char.ToLowerInvariant(c);

                isPreviousLetter = true;
                isPreviousUpper = isCurrentUpper;

                continue;
            }

            if (!isPreviousUpper && isCurrentUpper)
            {
                // word boundaries occur when we go from lower case to upper case
                upperCount = 1;
                output[outputIndex++] = '_';

            }
            else if (isPreviousUpper && !isCurrentUpper && upperCount >= 2)
            {
                // or when we go from upper to lower when there have been two or more previous upper characters
                upperCount = 0;
                output[outputIndex] = output[outputIndex - 1];
                output[outputIndex - 1] = '_';
                outputIndex++;
            }

            isPreviousUpper = isCurrentUpper;
            isPreviousLetter = isCurrentLetter;
            output[outputIndex++] = char.ToLowerInvariant(c);
        }

        return outputIndex;
    }
}
