using GTweens.Easings;
using System.Numerics;

namespace GTweens.Interpolators;

public sealed class SystemVector3Interpolator : IInterpolator<Vector3> {
    public static readonly SystemVector3Interpolator Instance = new();

    SystemVector3Interpolator() {

    }

    public Vector3 Evaluate(
        Vector3 initialValue,
        Vector3 finalValue,
        float time,
        EasingDelegate easingDelegate
        ) {
        return new Vector3(
            easingDelegate(initialValue.X, finalValue.X, time),
            easingDelegate(initialValue.Y, finalValue.Y, time),
            easingDelegate(initialValue.Z, finalValue.Z, time)
            );
    }

    public Vector3 Subtract(Vector3 initialValue, Vector3 finalValue) => finalValue - initialValue;

    public Vector3 Add(Vector3 initialValue, Vector3 finalValue) => finalValue + initialValue;
}