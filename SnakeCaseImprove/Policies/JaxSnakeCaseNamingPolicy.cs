using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SnakeCaseImprove;

public class JaxSnakeCaseNamingPolicy : JsonNamingPolicy
{
    private readonly bool _upperCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="JaxSnakeCaseNamingPolicy" /> class.
    /// </summary>
    /// <param name="upperCase">Whether the converted inputs should be in all upper case.</param>
    public JaxSnakeCaseNamingPolicy(bool upperCase = false)
    {
        _upperCase = upperCase;
    }

    /// <inheritdoc />
    public override string ConvertName(string input) => _upperCase
        ? input.Snake().ToUpperInvariant()
        : input.Snake();
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

        var outputSize = input.Length + (input.Length / 2);
        if (outputSize <= _stackAllocationCap)
        {
            Span<char> output = stackalloc char[outputSize];
            var length = SnakeCore(input, output);
            return new(output[..length]);
        }
        else
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
    }

    private static int SnakeCore(in ReadOnlySpan<char> input, in Span<char> output)
    {
        var index = 0;
        var state = InputState.Start;

        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                switch (state)
                {
                    case InputState.Upper:
                    {
                        var hasNext = i + 1 < input.Length;
                        if (i > 0 && hasNext)
                        {
                            var nextChar = input[i + 1];
                            if (!char.IsUpper(nextChar) && nextChar != '_')
                            {
                                output[index] = '_';
                                index++;
                            }
                        }

                        break;
                    }
                    case InputState.Lower:
                    {
                        output[index] = '_';
                        index++;
                        break;
                    }
                }

                var c = char.ToLowerInvariant(input[i]);

                output[index] = c;
                index++;

                state = InputState.Upper;
            }
            else if (input[i] == '_')
            {
                output[index] = '_';
                index++;
                state = InputState.Start;
            }
            else
            {
                output[index] = input[i];
                index++;
                state = InputState.Lower;
            }
        }

        return index;
    }

    private enum InputState
    {
        Start,
        Lower,
        Upper
    }
}
