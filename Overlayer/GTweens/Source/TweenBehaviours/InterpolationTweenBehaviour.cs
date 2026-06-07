using GTweens.Easings;
using GTweens.Enums;
using GTweens.Tweeners;

namespace GTweens.TweenBehaviours;

public sealed class InterpolationTweenBehaviour : TweenBehaviour {
    readonly List<ITweener> _tweeners = [];
    readonly List<ITweener> _playingTweeners = [];

    bool _durationCalculated;
    float _cachedCalculatedDuration;

    public override void Start(bool isCompletingInstantly) => StartTweeners();

    public override void Tick(float deltaTime) {
        for(int i = _playingTweeners.Count - 1; i >= 0; --i) {
            ITweener tweener = _playingTweeners[i];

            tweener.Tick(deltaTime);

            if(!tweener.IsPlaying) {
                _playingTweeners.RemoveAt(i);
            }
        }

        if(_playingTweeners.Count == 0) {
            MarkFinished();
        }
    }

    public override void Kill() {
        foreach(ITweener tweener in _playingTweeners) {
            tweener.Kill();
        }

        _playingTweeners.Clear();

        MarkFinished();
    }

    public override void Complete() {
        foreach(ITweener tweener in _playingTweeners) {
            if(!tweener.IsPlaying) {
                continue;
            }

            tweener.Complete();
        }

        _playingTweeners.Clear();

        MarkFinished();
    }

    public override void Reset(bool kill, ResetMode resetMode) {
        for(int i = _tweeners.Count - 1; i >= 0; --i) {
            ITweener tweener = _tweeners[i];

            tweener.Reset(resetMode);
        }

        MarkUnfinished();
    }

    public override void SetEasing(EasingDelegate easingFunction) {
        foreach(ITweener tweener in _tweeners) {
            tweener.SetEasing(easingFunction);
        }
    }

    public override float GetDuration() {
        if(_durationCalculated) {
            return _cachedCalculatedDuration;
        }

        _durationCalculated = true;

        _cachedCalculatedDuration = 0.0f;

        foreach(ITweener tweener in _tweeners) {
            _cachedCalculatedDuration += tweener.Duration;
        }

        return _cachedCalculatedDuration;
    }

    public override float GetElapsed() {
        float totalElapsed = 0.0f;

        foreach(ITweener tweener in _tweeners) {
            totalElapsed += tweener.Elapsed;
        }

        return totalElapsed;
    }

    public void Add(ITweener tweener) {
        if(tweener == null) {
            throw new ArgumentNullException(
                $"Tried to {nameof(Add)} a null {nameof(ITweener)} on {nameof(InterpolationTweenBehaviour)}"
            );
        }

        if(tweener.IsPlaying) {
            throw new ArgumentNullException(
                $"Tried to {nameof(Add)} a {nameof(ITweener)} on {nameof(InterpolationTweenBehaviour)} but it was already playing"
            );
        }

        if(_tweeners.Contains(tweener)) {
            throw new ArgumentNullException(
                $"Tried to {nameof(Add)} a {nameof(ITweener)} on {nameof(InterpolationTweenBehaviour)} but it was already added"
            );
        }

        _tweeners.Add(tweener);

        _durationCalculated = false;
    }

    void StartTweeners() {
        _playingTweeners.Clear();
        _playingTweeners.AddRange(_tweeners);

        for(int i = _playingTweeners.Count - 1; i >= 0; --i) {
            ITweener tweener = _playingTweeners[i];

            tweener.Start();

            if(!tweener.IsPlaying) {
                _playingTweeners.RemoveAt(i);
            }
        }

        if(_playingTweeners.Count == 0) {
            MarkFinished();
        }
    }
}
