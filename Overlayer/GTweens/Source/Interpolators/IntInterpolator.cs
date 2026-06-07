using GTweens.Easings;

namespace GTweens.Interpolators;

public sealed class IntInterpolator : IInterpolator<int> {
    public static readonly IntInterpolator Instance = new();

    IntInterpolator() {

    }

    public int Evaluate(
        int initialValue,
        int finalValue,
        float time,
        EasingDelegate easingDelegate
        ) => (int)easingDelegate(initialValue, finalValue, time);

    public int Subtract(int initialValue, int finalValue) => finalValue - initialValue;

    public int Add(int initialValue, int finalValue) => finalValue + initialValue;
}