using System.Text;
using System.Text.Json;

namespace SnakeCaseImprove;

public class RemoraSnakeCaseNamingPolicy : JsonNamingPolicy
{
    private readonly bool _upperCase;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RemoraSnakeCaseNamingPolicy" /> class.
    /// </summary>
    /// <param name="upperCase">Whether the converted names should be in all upper case.</param>
    public RemoraSnakeCaseNamingPolicy(bool upperCase = false)
    {
        _upperCase = upperCase;
    }

    /// <inheritdoc />
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var builder = new StringBuilder();

        var wordBoundaries = new List<int>();

        char? previous = null;
        for (var index = 0; index < name.Length; index++)
        {
            var c = name[index];

            if (previous.HasValue && char.IsUpper(previous.Value) && char.IsLower(c)) wordBoundaries.Add(index - 1);

            if (previous.HasValue && char.IsLower(previous.Value) && char.IsUpper(c)) wordBoundaries.Add(index);

            previous = c;
        }

        for (var index = 0; index < name.Length; index++)
        {
            var c = name[index];
            if (wordBoundaries.Contains(index) && index != 0) builder.Append('_');

            builder.Append(char.ToLowerInvariant(c));
        }

        return _upperCase
            ? builder.ToString().ToUpperInvariant()
            : builder.ToString();
    }
}