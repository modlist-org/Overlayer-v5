using GTweens.Easings;

namespace GTweens.Interpolators;

/// <summary>
/// Represents an interpolator for working with transitions between two values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of values to interpolate.</typeparam>
public interface IInterpolator<T> {
    /// <summary>
    /// Evaluates the intermediate value between the initial and final values based on the specified time and easing function.
    /// </summary>
    /// <param name="initialValue">The initial value.</param>
    /// <param name="finalValue">The final value.</param>
    /// <param name="time">The interpolation time (usually between 0 and 1).</param>
    /// <param name="easingFunction">The easing function to apply during interpolation.</param>
    /// <returns>The interpolated value between <paramref name="initialValue"/> and <paramref name="finalValue"/>.</returns>
    T Evaluate(T initialValue, T finalValue, float time, EasingDelegate easingFunction);

    /// <summary>
    /// Subtracts two values of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="initialValue">The initial value.</param>
    /// <param name="finalValue">The final value.</param>
    /// <returns>The result of subtracting <paramref name="finalValue"/> from <paramref name="initialValue"/>.</returns>
    T Subtract(T initialValue, T finalValue);

    /// <summary>
    /// Adds two values of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="initialValue">The initial value.</param>
    /// <param name="finalValue">The final value.</param>
    /// <returns>The result of adding <paramref name="finalValue"/> to <paramref name="initialValue"/>.</returns>
    T Add(T initialValue, T finalValue);
}