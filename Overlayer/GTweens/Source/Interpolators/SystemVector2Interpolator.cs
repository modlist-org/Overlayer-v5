using GTweens.Easings;
using System.Numerics;

namespace GTweens.Interpolators;

public sealed class SystemVector2Interpolator : IInterpolator<Vector2> {
    public static readonly SystemVector2Interpolator Instance = new();

    SystemVector2Interpolator() {

    }

    public Vector2 Evaluate(
        Vector2 initialValue,
        Vector2 finalValue,
        float time,
        EasingDelegate easingDelegate
        ) {
        return new Vector2(
            easingDelegate(initialValue.X, finalValue.X, time),
            easingDelegate(initialValue.Y, finalValue.Y, time)
            );
    }

    public Vector2 Subtract(Vector2 initialValue, Vector2 finalValue) => finalValue - initialValue;

    public Vector2 Add(Vector2 initialValue, Vector2 finalValue) => finalValue + initialValue;
}