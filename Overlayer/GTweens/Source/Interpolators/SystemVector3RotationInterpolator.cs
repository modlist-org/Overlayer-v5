// using Juce.Tweening.Easing;
// using Juce.Tweening.Utils;
// using System;
// using System.Numerics;
//
// namespace Juce.Tweening.Interpolators
// {
//     public class SystemVector3RotationInterpolator : IInterpolator<Vector3>
//     {
//         readonly RotationMode rotationMode;
//
//         public SystemVector3RotationInterpolator(RotationMode rotationMode)
//         {
//             this.rotationMode = rotationMode;
//         }
//
//         public Vector3 Evaluate(
//             Vector3 initialValue,
//             Vector3 finalValue,
//             float time,
//             EaseDelegate easeFunction
//             )
//         {
//             if (easeFunction == null)
//             {
//                 throw new ArgumentNullException($"Tried to Evaluate with a " +
//                     $"null {nameof(EaseDelegate)} on {nameof(SystemVector3Interpolator)}");
//             }
//
//             if (rotationMode == RotationMode.Fast)
//             {
//                 Vector3 deltaAngle = AngleUtils.DeltaAngle(initialValue, finalValue);
//
//                 finalValue = initialValue + deltaAngle;
//             }
//
//             return new Vector3(
//                 easeFunction(initialValue.X, finalValue.X, time),
//                 easeFunction(initialValue.Y, finalValue.Y, time),
//                 easeFunction(initialValue.Z, finalValue.Z, time)
//                 );
//         }
//
//         public Vector3 Subtract(Vector3 initialValue, Vector3 finalValue)
//         {
//             return finalValue - initialValue;
//         }
//
//         public Vector3 Add(Vector3 firstValue, Vector3 secondValue)
//         {
//             return secondValue + firstValue;
//         }
//     }
// }