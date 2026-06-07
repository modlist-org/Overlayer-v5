using GTweens.Delegates;
using GTweens.Interpolators;
using System.Numerics;

namespace GTweens.Tweeners;

public sealed class SystemVector2Tweener : Tweener<Vector2> {
    public SystemVector2Tweener(
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
              SystemVector2Interpolator.Instance,
              validation
              ) {
    }
}