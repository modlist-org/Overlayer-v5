using GTweens.Easings;
using GTweens.Enums;
using GTweens.TweenBehaviours;

namespace GTweens.Tweens;

/// <summary>
/// Represents the core item for building tweening animations.
/// </summary>
public sealed class GTween {
    public event Action OnStartAction;
    public event Action OnTickAction;
    public event Action OnLoopAction;
    public event Action OnResetAction;
    public event Action OnCompleteAction;
    public event Action OnKillAction;
    public event Action OnCompleteOrKillAction;
    public event Action<float> OnTimeScaleChangedAction;

    public ITweenBehaviour Behaviour { get; }
    public float TimeScale { get; private set; } = 1;

    public float Delay { get; private set; }

    public int Loops { get; private set; }
    public ResetMode LoopResetMode { get; private set; }

    public bool IsNested { get; set; }

    public bool IsPlaying { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsKilled { get; private set; }
    public bool IsCompletedOrKilled => IsCompleted || IsKilled;

    public bool IsAlive { get; set; }

    float _delayRemaining;
    int _loopsRemaining;

    public GTween(ITweenBehaviour behaviour) => Behaviour = behaviour;

    /// <summary>
    /// Starts the tween.
    /// </summary>
    /// <param name="isCompletingInstantly">Determines if the tween should complete instantly.</param>
    public void Start(bool isCompletingInstantly = false) {
        if(IsPlaying) {
            Kill();
        }

        IsPlaying = true;
        IsCompleted = false;
        IsKilled = false;

        _delayRemaining = Delay;
        _loopsRemaining = Loops;

        Behaviour.Start(isCompletingInstantly);

        OnStartAction?.Invoke();
    }

    /// <summary>
    /// Advances the tween by a given delta time.
    /// </summary>
    /// <param name="deltaTime">The elapsed time since the last update.</param>
    public void Tick(float deltaTime) {
        if(!IsPlaying) {
            return;
        }

        if(IsPaused) {
            return;
        }

        float deltaTimeWithTimeScale = TimeScale * deltaTime;

        if(_delayRemaining > 0f) {
            _delayRemaining -= deltaTime;
            return;
        }

        Behaviour.Tick(deltaTimeWithTimeScale);

        OnTickAction?.Invoke();

        bool isFinished = Behaviour.GetFinished();

        if(!isFinished) {
            return;
        }

        bool needsToLoop = _loopsRemaining > 0 && Behaviour.GetLoopable();

        if(needsToLoop) {
            Loop(LoopResetMode);
        } else {
            MarkFinished();
        }
    }

    /// <summary>
    /// Sets if the tween can continue updating or not. Has effect even if the tween has not startet playing.
    /// If you start a paused tween, it will not update until it's unpaused.
    /// </summary>
    public GTween SetPaused(bool paused) {
        IsPaused = paused;
        return this;
    }

    /// <summary>
    /// Instantly reaches the final state of the tween, and stops playing.
    /// </summary>
    public void Complete() {
        if(!IsPlaying && !IsCompleted) {
            Start(true);
        }

        Behaviour.Complete();

        _loopsRemaining = 0;

        MarkFinished();
    }

    /// <summary>
    /// Kills the tween. This means that the tween will stop playing, leaving it at its current state.
    /// </summary>
    public void Kill() {
        if(!IsPlaying) {
            return;
        }

        IsPlaying = false;
        IsCompleted = false;
        IsKilled = true;

        Behaviour.Kill();

        OnKillAction?.Invoke();
        OnCompleteOrKillAction?.Invoke();
    }

    /// <summary>
    /// Resets the tween, optionally killing it and specifying the reset mode.
    /// </summary>
    /// <param name="kill">Whether to kill the tween.</param>
    /// <param name="resetMode">The reset mode to use.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween Reset(bool kill, ResetMode resetMode = ResetMode.InitialValues) {
        if(kill) {
            Kill();
            IsPlaying = false;
        }

        IsCompleted = false;
        IsKilled = false;

        _delayRemaining = Delay;

        Behaviour.Reset(kill, resetMode);

        OnResetAction?.Invoke();

        return this;
    }

    /// <summary>
    /// Simulates the progress of a GTween animation for a specified duration.
    /// </summary>
    /// <param name="time">The simulated time in seconds.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween Simulate(float time) {
        if(!IsPlaying) {
            Start();
        }

        bool loops = Loops > 0;

        float simulationTime = loops ?
            time % Behaviour.GetDuration() :
            Math.Min(time, Behaviour.GetDuration());

        float progress = Behaviour.GetElapsed();

        while(simulationTime > 0) {
            Tick(simulationTime);

            float newProgress = Behaviour.GetElapsed();
            float tickElapsed = newProgress - progress;
            progress = newProgress;

            simulationTime -= tickElapsed;
        }

        return this;
    }

    /// <summary>
    /// Adds some delay (in seconds) at the begining of the tween.
    /// </summary>
    public GTween SetDelay(float delaySeconds) {
        Delay = delaySeconds;
        return this;
    }

    /// <summary>
    /// Sets the time scale for the tween, affecting its speed.
    /// By default time scale is 1.0f. If you decrease this number, the tween
    /// will update slower. If you increase it, the tween will update faster.
    /// </summary>
    /// <param name="timeScale">The time scale to set.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween SetTimeScale(float timeScale) {
        TimeScale = timeScale;
        OnTimeScaleChangedAction?.Invoke(timeScale);
        return this;
    }

    /// <summary>
    /// Sets the easing function for the tween.
    /// </summary>
    /// <param name="easingFunction">The custom easing function to use.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween SetEasing(EasingDelegate easingFunction) {
        Behaviour.SetEasing(easingFunction);
        return this;
    }

    /// <summary>
    /// Sets the predefined easing function for the tween.
    /// </summary>
    /// <param name="easing">The predefined easing to use.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween SetEasing(Easing easing) => SetEasing(PresetEasingDelegateFactory.GetEaseDelegate(easing));

    /// <summary>
    /// Sets the number of loops and the reset mode for the tween.
    /// </summary>
    /// <param name="loops">The number of loops.</param>
    /// <param name="resetMode">The reset mode to use when looping.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween SetLoops(int loops, ResetMode resetMode = ResetMode.InitialValues) {
        Loops = Math.Max(loops, 0);
        LoopResetMode = resetMode;
        return this;
    }

    /// <summary>
    /// Sets the tween to have and almost infinite amount of loops with the specified reset mode.
    /// </summary>
    /// <param name="resetMode">The reset mode to use when looping.</param>
    /// <returns>The current GTween instance for method chaining.</returns>
    public GTween SetMaxLoops(ResetMode resetMode = ResetMode.InitialValues) => SetLoops(int.MaxValue, resetMode);

    /// <summary>
    /// Calculates the total duration of the tween.
    /// </summary>
    public float GetDuration() => Behaviour.GetDuration();

    /// <summary>
    /// Calculates the time elapsed since the tween started playing.
    /// </summary>
    public float GetElapsed() {
        if(!IsPlaying && !IsCompleted) {
            return 0f;
        }

        if(!IsPlaying && IsCompleted) {
            return GetDuration();
        }

        return Behaviour.GetElapsed();
    }

    /// <summary>
    /// Gets the time left remaining on the tween (duration - elapsed).
    /// </summary>
    public float GetRemaining() {
        if(!IsPlaying && !IsCompleted) {
            return GetDuration();
        }

        if(!IsPlaying && IsCompleted) {
            return 0f;
        }

        return Behaviour.GetRemaining();
    }

    public GTween OnStart(Action action) {
        OnStartAction += action;
        return this;
    }

    public GTween OnTick(Action action) {
        OnTickAction += action;
        return this;
    }

    public GTween OnLoop(Action action) {
        OnLoopAction += action;
        return this;
    }

    public GTween OnReset(Action action) {
        OnResetAction += action;
        return this;
    }

    public GTween OnComplete(Action action) {
        OnCompleteAction += action;
        return this;
    }

    public GTween OnKill(Action action) {
        OnKillAction += action;
        return this;
    }

    public GTween OnCompleteOrKill(Action action) {
        OnCompleteOrKillAction += action;
        return this;
    }

    public GTween OnTimeScaleChanged(Action<float> action) {
        OnTimeScaleChangedAction += action;
        return this;
    }

    void Loop(ResetMode loopResetMode) {
        bool needsToLoop = _loopsRemaining > 0;

        if(!needsToLoop || !Behaviour.GetLoopable()) {
            return;
        }

        --_loopsRemaining;

        Reset(kill: false, loopResetMode);

        IsPlaying = true;
        IsCompleted = false;
        IsKilled = false;

        Behaviour.Start(false);

        OnLoopAction?.Invoke();
    }

    void MarkFinished() {
        if(!IsPlaying) {
            return;
        }

        IsPlaying = false;
        IsCompleted = true;
        IsKilled = false;

        Behaviour.Complete();

        OnCompleteAction?.Invoke();
        OnCompleteOrKillAction?.Invoke();
    }
}
