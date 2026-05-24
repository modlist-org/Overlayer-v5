using Overlayer.Tag.Core;
using Overlayer.Tag.Diagnostics;
using Overlayer.Tag.Runtime;
using Overlayer.TextEngine.Parse;
using System.Linq.Expressions;

namespace Overlayer.Tag.Compile;

public static class Compiler {
    public static CompiledPlaceholder Compile(
        TagCore tag,
        ParsedTag parsed
    ) {
        var diagnostics = new List<CompileDiagnostic>();

        var placeholder = new Placeholder(parsed.Name, parsed.Args);

        var context = new DiagnosticContext(
            parsed.Name,
            parsed.Index,
            parsed.Length
        );

        var sig = SignatureResolver.Resolve(tag, placeholder, diagnostics, context);

        var expr = ExpressionBuilder.Build(tag, sig, diagnostics);

        var lambda = Expression.Lambda<Func<string>>(expr);

        return new CompiledPlaceholder(
            lambda.Compile(),
            [.. diagnostics]
        );
    }
}