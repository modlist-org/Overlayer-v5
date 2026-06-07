using GTweens.Enums;
using System.Numerics;

namespace GTweens.Extensions;

internal static class AngleExtensions {
    public static float Clamp360(float eulerAngles) {
        float result = eulerAngles - ((float)Math.Ceiling(eulerAngles / 360f) * 360f);

        if(result < 0) {
            result += 360f;
        }

        return result;
    }

    public static Vector3 Clamp360(Vector3 eulerAngles) {
        return new Vector3(
            Clamp360(eulerAngles.X),
            Clamp360(eulerAngles.Y),
            Clamp360(eulerAngles.Z)
            );
    }

    /// <summary>
    /// Calculates the shortest difference between two given angles.
    /// </summary>
    public static float DeltaAngle(float current, float target) {
        float difference = target - current;

        float delta = MathExtensions.Repeat(difference, 360f);

        if(delta > 180.0F) {
            delta -= 360.0F;
        }

        return delta;
    }

    public static Vector3 DeltaAngle(Vector3 current, Vector3 target) {
        return new Vector3(
            DeltaAngle(current.X, target.X),
            DeltaAngle(current.Y, target.Y),
            DeltaAngle(current.Z, target.Z)
            );
    }

    public static float GetDestinationAngleDegrees(float origin, float destination, RotationMode mode) {
        switch(mode) {
            case RotationMode.ShortestDistance: {
                float clampedOrigin = Clamp360(origin);
                float clampedDestination = Clamp360(destination);

                float deltaAngle = DeltaAngle(clampedOrigin, clampedDestination);

                return origin + deltaAngle;
            }

            default:
            case RotationMode.TotalDistance: {
                return destination;
            }
        }
    }

    public static float GetDestinationAngleRadiants(float origin, float destination, RotationMode mode) {
        return GetDestinationAngleDegrees(
            origin * MathExtensions.Rad2Deg,
            destination * MathExtensions.Rad2Deg,
            mode
        ) * MathExtensions.Deg2Rad;
    }
}