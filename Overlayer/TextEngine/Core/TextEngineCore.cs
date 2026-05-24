using Overlayer.Tag.Core;
using Overlayer.Tag.Diagnostics;
using Overlayer.TextEngine.Parse;
using Overlayer.TextEngine.Runtime;
using System.Text;

namespace Overlayer.TextEngine.Core;

public sealed class TextEngineCore {
    private string text;

    public string Text {
        get => text;
        set {
            if(text == value) {
                return;
            }

            text = value;
            Compile();
        }
    }

    public CompiledSegment[] Segments;

    public CompileDiagnostic[] GetDiagnostics()
        => [.. Segments.SelectMany(s => s.Replacer.Compiled.Diagnostics)];

    private void Compile() {
        if(string.IsNullOrEmpty(text)) {
            Segments = [];
            return;
        }

        var tags = Parser.Parse(text);

        if(tags.Count == 0) {
            Segments = [];
            return;
        }

        Segments = new CompiledSegment[tags.Count];

        for(int i = 0; i < tags.Count; i++) {
            var t = tags[i];

            Segments[i] = new CompiledSegment(
                t.Index,
                t.Length,
                new Tag.Runtime.Replacer {
                    Parsed = t
                }
            );
        }
    }

    public string Get() {
        if(Segments == null || Segments.Length == 0) {
            return text ?? string.Empty;
        }

        var sb = new StringBuilder(text.Length);
        int last = 0;

        for(int i = 0; i < Segments.Length; i++) {
            var s = Segments[i];

            sb.Append(text, last, s.Index - last);
            sb.Append(s.Replacer.Get());

            last = s.Index + s.Length;
        }

        sb.Append(text, last, text.Length - last);

        return sb.ToString();
    }
}