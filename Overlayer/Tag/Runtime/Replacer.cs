using Overlayer.TextEngine.Parse;

namespace Overlayer.Tag.Runtime;

public sealed class Replacer {
    public CompiledPlaceholder Compiled { get; private set; } = EmptyDelegates.EmptyCompiled;

    private ParsedTag parsed;

    public ParsedTag Parsed {
        get => parsed;
        set {
            if(parsed.Equals(value)) {
                return;
            }

            parsed = value;
            Compiled = TagCache.GetOrCompile(parsed);
        }
    }

    public bool IsValid => Compiled.IsValid;

    public string Get() => Compiled.Get();
}