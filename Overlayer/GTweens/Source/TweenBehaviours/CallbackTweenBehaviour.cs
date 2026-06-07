using GTweens.Enums;

namespace GTweens.TweenBehaviours;

public sealed class CallbackTweenBehaviour : TweenBehaviour {
    readonly Action _action;
    readonly bool _callIfCompletingInstantly;

    public CallbackTweenBehaviour(Action action, bool callIfCompletingInstantly) {
        _action = action;
        _callIfCompletingInstantly = callIfCompletingInstantly;
    }

    public override void Start(bool isCompletingInstantly) {
        bool canCall = !isCompletingInstantly || _callIfCompletingInstantly;

        if(canCall) {
            _action?.Invoke();
        }

        MarkFinished();
    }

    public override void Reset(bool kill, ResetMode loopResetMode) => MarkUnfinished();

    public override float GetDuration() => 0f;
    public override float GetElapsed() => 0f;
    public override bool GetLoopable() => false;
}
