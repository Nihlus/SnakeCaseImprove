using System.Text.Json;

namespace SnakeCaseImprove;

public class JsonNetStackallocSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var index = 0;
        var state = SeparatedCaseState.Start;

        var width = name.Length + name.Length / 2;

        Span<char> span = stackalloc char[width];

        for (var i = 0; i < name.Length; i++)
            if (name[i] == ' ')
            {
                if (state != SeparatedCaseState.Start) state = SeparatedCaseState.NewWord;
            }
            else if (char.IsUpper(name[i]))
            {
                switch (state)
                {
                    case SeparatedCaseState.Upper:
                        var hasNext = i + 1 < name.Length;
                        if (i > 0 && hasNext)
                        {
                            var nextChar = name[i + 1];
                            if (!char.IsUpper(nextChar) && nextChar != '_')
                            {
                                span[index] = '_';
                                index++;
                            }
                        }

                        break;
                    case SeparatedCaseState.Lower:
                    case SeparatedCaseState.NewWord:
                        span[index] = '_';
                        index++;
                        break;
                }

                var c = char.ToLowerInvariant(name[i]);

                span[index] = c;
                index++;

                state = SeparatedCaseState.Upper;
            }
            else if (name[i] == '_')
            {
                span[index] = '_';
                index++;
                state = SeparatedCaseState.Start;
            }
            else
            {
                if (state == SeparatedCaseState.NewWord)
                {
                    span[index] = '_';
                    index++;
                }

                span[index] = name[i];
                index++;
                state = SeparatedCaseState.Lower;
            }

        return new string(span[..index]);
    }

    private enum SeparatedCaseState
    {
        Start,
        Lower,
        Upper,
        NewWord
    }
}