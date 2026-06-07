using GTweens.Easings;
using GTweens.Extensions;
using System.Drawing;

namespace GTweens.Interpolators;

public sealed class SystemColorInterpolator : IInterpolator<Color> {
    public static readonly SystemColorInterpolator Instance = new();

    SystemColorInterpolator() {

    }

    public Color Evaluate(
        Color initialValue,
        Color finalValue,
        float time,
        EasingDelegate easingDelegate
        ) {
        return SystemColorExtensions.FromRgba(
            easingDelegate!(initialValue.A, finalValue.A, time),
            easingDelegate(initialValue.R, finalValue.R, time),
            easingDelegate(initialValue.G, finalValue.G, time),
            easingDelegate(initialValue.B, finalValue.B, time)
            );
    }

    public Color Subtract(Color initialValue, Color finalValue) {
        return Color.FromArgb(
            finalValue.A - initialValue.A,
            finalValue.R - initialValue.R,
            finalValue.G - initialValue.G,
            finalValue.B - initialValue.B
        );
    }

    public Color Add(Color initialValue, Color finalValue) {
        return Color.FromArgb(
            finalValue.A + initialValue.A,
            finalValue.R + initialValue.R,
            finalValue.G + initialValue.G,
            finalValue.B + initialValue.B
        );
    }
}