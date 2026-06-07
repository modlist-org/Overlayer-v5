using GTweens.Easings;
using System.Numerics;

namespace GTweens.Interpolators;

public sealed class SystemVector4Interpolator : IInterpolator<Vector4> {
    public static readonly SystemVector4Interpolator Instance = new();

    SystemVector4Interpolator() {

    }

    public Vector4 Evaluate(
        Vector4 initialValue,
        Vector4 finalValue,
        float time,
        EasingDelegate easingDelegate
        ) {
        return new Vector4(
            easingDelegate(initialValue.X, finalValue.X, time),
            easingDelegate(initialValue.Y, finalValue.Y, time),
            easingDelegate(initialValue.Z, finalValue.Z, time),
            easingDelegate(initialValue.W, finalValue.W, time)
            );
    }

    public Vector4 Subtract(Vector4 initialValue, Vector4 finalValue) => finalValue - initialValue;

    public Vector4 Add(Vector4 initialValue, Vector4 finalValue) => finalValue + initialValue;
}