using GTweens.Easings;
using GTweens.Enums;
using GTweens.Extensions;
using GTweens.Tweens;

namespace GTweens.TweenBehaviours;

public sealed class SequenceTweenBehaviour : TweenBehaviour {
    public IReadOnlyList<GTween> Tweens => _tweens;

    readonly List<GTween> _tweens = [];
    readonly List<GTween> _playingTweens = [];

    bool _durationCalculated;
    float _cachedCalculatedDuration;

    public override void Start(bool isCompletingInstantly) => StartTweens(isCompletingInstantly);

    public override void Tick(float deltaTime) {
        if(_playingTweens.Count == 0) {
            MarkFinished();
            return;
        }

        GTween gTween = _playingTweens[0];

        gTween.Tick(deltaTime);

        if(gTween.IsPlaying) {
            return;
        }

        _playingTweens.RemoveAt(0);

        if(_playingTweens.Count > 0) {
            GTween nextGTween = _playingTweens[0];

            nextGTween.Start();
        } else {
            Tick(deltaTime);
        }
    }

    public override void Kill() {
        foreach(GTween tween in _playingTweens) {
            tween.Kill();
        }

        _playingTweens.Clear();

        MarkFinished();
    }

    public override void Complete() {
        foreach(GTween tween in _tweens) {
            if(tween.IsCompleted) {
                continue;
            }

            if(!tween.IsPlaying) {
                tween.Start(isCompletingInstantly: true);
            }

            tween.Complete();
        }

        _playingTweens.Clear();

        MarkFinished();
    }

    public override void Reset(bool kill, ResetMode resetMode) {
        for(int i = _tweens.Count - 1; i >= 0; --i) {
            GTween gTween = _tweens[i];

            gTween.Reset(kill, resetMode);
        }

        MarkUnfinished();
    }

    public override void SetEasing(EasingDelegate easingFunction) {
        foreach(GTween tween in _tweens) {
            tween.SetEasing(easingFunction);
        }
    }

    public override float GetDuration() {
        if(_durationCalculated) {
            return _cachedCalculatedDuration;
        }

        _durationCalculated = true;

        _cachedCalculatedDuration = 0.0f;

        foreach(GTween tween in _tweens) {
            _cachedCalculatedDuration += tween.GetDuration();
        }

        return _cachedCalculatedDuration;
    }

    public override float GetElapsed() {
        float totalDuration = 0.0f;

        foreach(GTween tween in _tweens) {
            totalDuration += tween.GetElapsed();
        }

        return totalDuration;
    }

    public void Add(GTween gTween) {
        if(gTween.IsPlayingOrCompletedOrNested()) {
            return;
        }

        gTween.IsNested = true;

        _tweens.Add(gTween);

        _durationCalculated = false;
    }

    public void Remove(GTween gTween) {
        if(gTween.IsPlayingOrCompleted()) {
            return;
        }

        bool found = _tweens.Remove(gTween);

        if(!found) {
            return;
        }

        gTween.IsNested = false;

        _durationCalculated = false;
    }

    void StartTweens(bool isCompletingInstantly) {
        _playingTweens.Clear();
        _playingTweens.AddRange(_tweens);

        if(_playingTweens.Count > 0) {
            GTween gTween = _playingTweens[0];
            gTween.Start(isCompletingInstantly);
        } else {
            MarkFinished();
        }
    }
}
