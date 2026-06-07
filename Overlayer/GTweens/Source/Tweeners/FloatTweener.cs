using GTweens.Delegates;
using GTweens.Interpolators;

namespace GTweens.Tweeners;

public sealed class FloatTweener : Tweener<float> {
    public FloatTweener(
        Getter currentValueGetter,
        Setter setter,
        Getter to,
        float duration,
        ValidationDelegates.Validation validation
    )
        : base(
            currentValueGetter,
            setter,
            to,
            duration,
            FloatInterpolator.Instance,
            validation
        ) {
    }
}