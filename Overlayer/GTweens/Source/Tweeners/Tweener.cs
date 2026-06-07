using GTweens.Delegates;
using GTweens.Easings;
using GTweens.Enums;
using GTweens.Extensions;
using GTweens.Interpolators;

namespace GTweens.Tweeners;

public abstract class Tweener<T> : ITweener {
    public delegate void Setter(T currentValue);
    public delegate T Getter();

    public bool IsPlaying { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsKilled { get; private set; }
    public bool IsCompletedOrKilled => IsCompleted || IsKilled;

    public float Duration { get; }
    public float Elapsed { get; private set; }
    public float Remaining => Math.Max(Duration - Elapsed, 0f);

    readonly Getter _getter;
    readonly Setter _setter;
    readonly Getter _to;
    readonly ValidationDelegates.Validation _validation;

    readonly IInterpolator<T> _interpolator;

    bool _hasFirstTimeValues;
    T _firstTimeInitialValue = default!;
    T _firstTimeFinalValue = default!;

    T _initialValue = default!;
    T _finalValue = default!;
    T _currentValue = default!;

    EasingDelegate _easingFunction = PresetEasingDelegateFactory.GetEaseDelegate(Easing.Linear);

    public Tweener(
        Getter getter,
        Setter setter,
        Getter to,
        float duration,
        IInterpolator<T> interpolator,
        ValidationDelegates.Validation validation
        ) {
        _getter = getter;
        _setter = setter;
        _to = to;
        _interpolator = interpolator;
        _validation = validation;

        Duration = Math.Max(duration, 0.0f);
    }

    public void Start() {
        if(IsPlaying) {
            return;
        }

        IsPlaying = true;
        IsCompleted = false;
        IsKilled = false;

        Elapsed = 0.0f;

        bool valid = Validate();

        if(!valid) {
            Kill();
            return;
        }

        _initialValue = _getter.Invoke();

        GetFirstTimeValues();

        CompleteIfInstant();
    }

    public void Reset(ResetMode mode) {
        bool valid = Validate();

        if(!valid) {
            Kill();
            return;
        }

        GetFirstTimeValues();

        IsCompleted = false;
        IsKilled = false;
        Elapsed = 0.0f;

        switch(mode) {
            default:
            case ResetMode.InitialValues: {
                _setter(_firstTimeInitialValue);
                _finalValue = _firstTimeFinalValue;
            }
            break;

            case ResetMode.IncrementalValues: {
                T difference = _interpolator.Subtract(_firstTimeInitialValue, _firstTimeFinalValue);
                _finalValue = _interpolator.Add(_currentValue, difference);
            }
            break;

            case ResetMode.PingPong: {
                _finalValue = _initialValue;
            }
            break;
        }
    }

    public void Tick(float deltaTime) {
        if(!IsPlaying) {
            return;
        }

        bool valid = Validate();

        if(!valid) {
            Kill();
            return;
        }

        Elapsed = Math.Min(Duration, Elapsed + deltaTime);

        if(Elapsed < Duration) {
            float timeNormalized = MathExtensions.SafeDivide(Elapsed, Duration);

            _currentValue = _interpolator.Evaluate(
                _initialValue,
                _finalValue,
                timeNormalized,
                _easingFunction
            );

            _setter(_currentValue);
        } else {
            Complete();
        }
    }

    public void Complete() {
        bool valid = Validate();

        if(!valid) {
            return;
        }

        GetFirstTimeValues();

        T newValue = _interpolator.Evaluate(
            _initialValue,
            _finalValue,
            1.0f,
            _easingFunction
        );

        _setter(newValue);

        IsPlaying = false;
        IsCompleted = true;
    }

    public void Kill() {
        IsKilled = true;
        IsPlaying = false;
    }

    public void SetEasing(EasingDelegate easingFunction) => _easingFunction = easingFunction;

    void CompleteIfInstant() {
        if(!IsPlaying) {
            return;
        }

        bool isInstant = Duration == 0.0f;

        if(isInstant) {
            Complete();
        }
    }

    bool Validate() => _validation.Invoke();

    void GetFirstTimeValues() {
        if(_hasFirstTimeValues) {
            return;
        }

        _hasFirstTimeValues = true;

        _finalValue = _to.Invoke();

        _firstTimeInitialValue = _initialValue;
        _firstTimeFinalValue = _finalValue;
    }
}