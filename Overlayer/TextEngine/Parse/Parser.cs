using System.Text.RegularExpressions;

namespace Overlayer.TextEngine.Parse;

public static class Parser {
    private static readonly Regex ColonPattern =
        new(@"\{(?<name>[A-Za-z0-9_]+):(?<arg>[^}]*)\}", RegexOptions.Compiled);
    private static readonly Regex FuncPattern =
        new(@"\{(?<name>[A-Za-z0-9_]+)\((?<args>[^}]*)\)\}", RegexOptions.Compiled);

    public static List<ParsedTag> Parse(string input) {
        var result = new List<ParsedTag>();

        var matches = ColonPattern.Matches(input)
            .Cast<Match>()
            .Concat(FuncPattern.Matches(input).Cast<Match>())
            .OrderBy(m => m.Index);

        foreach(var m in matches) {
            string name = m.Groups["name"].Value;
            string argsRaw = m.Groups["arg"].Success ? m.Groups["arg"].Value : m.Groups["args"].Value;

            string[] args;

            if(string.IsNullOrWhiteSpace(argsRaw)) {
                args = [];
            } else {
                args = argsRaw.Split(',');

                for(int i = 0; i < args.Length; i++) {
                    args[i] = args[i].Trim();
                }
            }

            result.Add(new ParsedTag(
                input.Substring(m.Index, m.Length),
                name,
                args,
                m.Index,
                m.Length
            ));
        }

        return result;
    }
}