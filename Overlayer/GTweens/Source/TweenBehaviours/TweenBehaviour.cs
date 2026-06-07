using GTweens.Easings;
using GTweens.Enums;

namespace GTweens.TweenBehaviours;

public abstract class TweenBehaviour : ITweenBehaviour {
    bool _finished;

    public bool GetFinished() => _finished;

    protected void MarkFinished() => _finished = true;
    protected void MarkUnfinished() => _finished = false;

    public float GetRemaining() {
        float duration = GetDuration();
        float elapsed = GetElapsed();

        return Math.Max(duration - elapsed, 0f);
    }

    public abstract float GetDuration();
    public abstract float GetElapsed();
    public virtual bool GetLoopable() => true;

    public virtual void Start(bool isCompletingInstantly) { }
    public virtual void Tick(float deltaTime) { }
    public virtual void Kill() { }
    public virtual void Complete() { }
    public virtual void Reset(bool kill, ResetMode loopResetMode) { }
    public virtual void SetEasing(EasingDelegate easingFunction) { }
}