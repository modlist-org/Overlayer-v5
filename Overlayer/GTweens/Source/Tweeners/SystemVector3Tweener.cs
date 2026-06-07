using GTweens.Delegates;
using GTweens.Interpolators;
using System.Numerics;

namespace GTweens.Tweeners;

public sealed class SystemVector3Tweener : Tweener<Vector3> {
    public SystemVector3Tweener(
        Getter currValueGetter,
        Setter setter,
        Getter to,
        float duration,
        ValidationDelegates.Validation validation
        )
        : base(
              currValueGetter,
              setter,
              to,
              duration,
              SystemVector3Interpolator.Instance,
              validation
              ) {
    }
}