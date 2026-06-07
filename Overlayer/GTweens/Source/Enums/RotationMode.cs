namespace GTweens.Enums;

/// <summary>
/// Specifies different rotation modes for tweening operations.
/// </summary>
public enum RotationMode {
    /// <summary>
    /// Rotates objects using the shortest distance between the initial and final angles.
    /// </summary>
    ShortestDistance,

    /// <summary>
    /// Rotates objects based on the total angular distance, which may involve multiple rotations.
    /// </summary>
    TotalDistance,
}