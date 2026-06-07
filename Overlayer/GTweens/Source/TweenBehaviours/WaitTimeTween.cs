using GTweens.Enums;

namespace GTweens.TweenBehaviours;

public sealed class WaitTimeTweenBehaviour : TweenBehaviour {
    readonly float _durationSeconds;

    float _elapsedSeconds;

    public WaitTimeTweenBehaviour(float durationSeconds) => _durationSeconds = durationSeconds;

    public override void Start(bool isCompletingInstantly) => _elapsedSeconds = 0f;

    public override void Tick(float deltaTime) {
        _elapsedSeconds += deltaTime;

        if(_elapsedSeconds >= _durationSeconds) {
            MarkFinished();
        }
    }

    public override void Complete() {
        _elapsedSeconds = _durationSeconds;
        MarkFinished();
    }

    public override void Reset(bool kill, ResetMode loopResetMode) {
        _elapsedSeconds = 0f;
        MarkUnfinished();
    }

    public override float GetDuration() => _durationSeconds;

    public override float GetElapsed() => _elapsedSeconds;
}
