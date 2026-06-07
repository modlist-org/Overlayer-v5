namespace GTweens.Enums;

/// <summary>
/// Specifies different modes for resetting the values of a tween animation.
/// </summary>
public enum ResetMode {
    /// <summary>
    /// Resets the values to their initial state, as they were when the tween animation started.
    /// </summary>
    InitialValues,

    /// <summary>
    /// Resets the values by using the difference between initial and final values, effectively incrementing the new initial and final values.
    /// </summary>
    IncrementalValues,

    /// <summary>
    /// Each time reverses the animation initial and final values, in a ping-pong fashion.
    /// </summary> 
    PingPong,

    /// <summary>
    /// Leaves the values unchanged, maintaining their current state.
    /// </summary>
    CurrentValues,
}