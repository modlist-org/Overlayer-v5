using GTweens.Tweens;
using System.Diagnostics;

namespace GTweens.Contexts;

/// <summary>
/// Manages and updates a collection of GTweens.
/// </summary>
public sealed class GTweensContext {
    /// <summary>
    /// Gets or sets the time scale at which the tweens should play.
    /// </summary>
    public float TimeScale { get; set; } = 1f;

    /// <summary>
    /// Gets the duration of the last tweens tick in milliseconds.
    /// </summary>
    public float TickDurationMs { get; private set; }

    readonly List<GTween> _aliveTweens = [];
    readonly List<GTween> _tweensToAdd = [];
    readonly List<GTween> _tweensToRemove = [];

    readonly Stopwatch _updateStopwatch = new();

    /// <summary>
    /// Plays a GTween within the context.
    /// </summary>
    /// <param name="gTween">The GTween to play.</param>
    public void Play(GTween gTween) {
        if(gTween.IsNested) {
            return;
        }

        if(gTween.IsAlive) {
            TryStartTween(gTween);
            return;
        }

        gTween.IsAlive = true;

        _tweensToAdd.Add(gTween);

        TryStartTween(gTween);
    }

    /// <summary>
    /// Updates the context and all managed tweens.
    /// </summary>
    /// <param name="deltaTime">The elapsed time since the last update in seconds.</param>
    public void Tick(float deltaTime) {
        float scaledDeltaTime = deltaTime * TimeScale;

        _updateStopwatch.Restart();

        foreach(GTween tween in _tweensToAdd) {
            _aliveTweens.Add(tween);
        }

        _tweensToAdd.Clear();

        foreach(GTween tween in _aliveTweens) {
            if(tween.IsPlaying) {
                tween.Tick(scaledDeltaTime);
            } else {
                _tweensToRemove.Add(tween);
            }
        }

        foreach(GTween tween in _tweensToRemove) {
            tween.IsAlive = false;

            _aliveTweens.Remove(tween);
            _tweensToAdd.Remove(tween);
        }

        _tweensToRemove.Clear();

        _updateStopwatch.Stop();

        TickDurationMs = _updateStopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Clears all tweens from the context.
    /// </summary>
    public void Clear() {
        _aliveTweens.Clear();
        _tweensToAdd.Clear();
        _tweensToRemove.Clear();
    }

    void TryStartTween(GTween gTween) {
        if(!gTween.IsPlaying) {
            gTween.Start();
        }
    }
}