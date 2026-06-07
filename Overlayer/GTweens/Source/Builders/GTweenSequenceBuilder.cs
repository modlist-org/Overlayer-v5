using GTweens.TweenBehaviours;
using GTweens.Tweens;

namespace GTweens.Builders;

/// <summary>
/// Builder class for creating sequences of tweens.
/// </summary>
public sealed class GTweenSequenceBuilder {
    readonly SequenceTweenBehaviour _sequenceTweenBehaviour;
    readonly GTween _gTween;

    bool _creatingGroupTween;
    GroupTweenBehaviour _groupTweenBehaviour;

    GTweenSequenceBuilder() {
        _sequenceTweenBehaviour = new SequenceTweenBehaviour();
        _gTween = new GTween(_sequenceTweenBehaviour);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GTweenSequenceBuilder"/>.
    /// </summary>
    /// <returns>A new instance of the builder.</returns>
    public static GTweenSequenceBuilder New() => new();

    /// <summary>
    /// Adds the given tween to the end of the Sequence. This tween will play after all the previous tweens have finished.
    /// </summary>
    /// <param name="gTween">The GTween to append to the sequence.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder Append(GTween gTween) {
        _creatingGroupTween = false;

        _sequenceTweenBehaviour.Add(gTween);

        return this;
    }

    /// <summary>
    /// Inserts the given tween at the same time position of the last tween added to the Sequence.
    /// This tween will play at the same time as the previous tween.
    /// </summary>
    /// <param name="gTween">The GTween to join with the sequence.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder Join(GTween gTween) {
        if(_creatingGroupTween) {
            _groupTweenBehaviour!.Add(gTween);
            return this;
        }

        _creatingGroupTween = true;

        _groupTweenBehaviour = new GroupTweenBehaviour();

        if(_sequenceTweenBehaviour.Tweens.Count > 0) {
            GTween previousTween = _sequenceTweenBehaviour.Tweens[^1];
            _sequenceTweenBehaviour.Remove(previousTween);
            _groupTweenBehaviour.Add(previousTween);
        }

        _groupTweenBehaviour.Add(gTween);

        _sequenceTweenBehaviour.Add(new GTween(_groupTweenBehaviour));

        return this;
    }

    /// <summary>
    /// Appends a callback action to the end of the sequence.
    /// </summary>
    /// <param name="callback">The callback action to append.</param>
    /// <param name="callIfCompletingInstantly">Whether to call the callback if the tween is asked to complete instantly.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder AppendCallback(Action callback, bool callIfCompletingInstantly = true) {
        CallbackTweenBehaviour callbackTweenBehaviour = new(callback, callIfCompletingInstantly);
        Append(new GTween(callbackTweenBehaviour));

        return this;
    }

    /// <summary>
    /// Inserts the given callback at the same time position of the last tween added to the Sequence.
    /// This tween will play at the same time as the previous tween.
    /// </summary>
    /// <param name="callback">The callback action to append.</param>
    /// <param name="callIfCompletingInstantly">Whether to call the callback if the tween is asked to complete instantly.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder JoinCallback(Action callback, bool callIfCompletingInstantly = true) {
        CallbackTweenBehaviour callbackTweenBehaviour = new(callback, callIfCompletingInstantly);
        Join(new GTween(callbackTweenBehaviour));

        return this;
    }

    /// <summary>
    /// Appends a time delay to the end of the sequence.
    /// </summary>
    /// <param name="timeSeconds">The duration of the time delay in seconds.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder AppendTime(float timeSeconds) {
        WaitTimeTweenBehaviour timeTweenBehaviour = new(timeSeconds);
        Append(new GTween(timeTweenBehaviour));

        return this;
    }

    /// <summary>
    /// Inserts the given time delay at the same time position of the last tween added to the Sequence.
    /// This tween will play at the same time as the previous tween.
    /// </summary>
    /// <param name="timeSeconds">The duration of the time delay in seconds.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder JoinTime(float timeSeconds) {
        WaitTimeTweenBehaviour timeTweenBehaviour = new(timeSeconds);
        Join(new GTween(timeTweenBehaviour));

        return this;
    }

    /// <summary>
    /// Provides a new GTweenSequenceBuilder for building a sequence, and then adds it to the end of the sequence.
    /// </summary>
    /// <param name="createSequence">An action that defines the nested sequence using a new GTweenSequenceBuilder.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder AppendSequence(Action<GTweenSequenceBuilder> createSequence) {
        GTweenSequenceBuilder sequenceBuilder = New();
        createSequence.Invoke(sequenceBuilder);
        Append(sequenceBuilder.Build());

        return this;
    }

    /// <summary>
    /// Provides a new GTweenSequenceBuilder for building a sequence, and then inserts it
    /// at the same time position of the last tween added to the Sequence.
    /// </summary>
    /// <param name="createSequence">An action that defines the nested sequence using a new GTweenSequenceBuilder.</param>
    /// <returns>The current instance of the builder.</returns>
    public GTweenSequenceBuilder JoinSequence(Action<GTweenSequenceBuilder> createSequence) {
        GTweenSequenceBuilder sequenceBuilder = New();
        createSequence.Invoke(sequenceBuilder);
        Join(sequenceBuilder.Build());

        return this;
    }

    /// <summary>
    /// Builds and returns the final GTween representing the sequence.
    /// </summary>
    /// <returns>The GTween representing the built sequence.</returns>
    public GTween Build() => _gTween;
}