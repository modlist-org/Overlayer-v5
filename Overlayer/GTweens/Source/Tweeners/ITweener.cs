using GTweens.Easings;
using GTweens.Enums;

namespace GTweens.Tweeners;

public interface ITweener {
    float Duration { get; }
    float Elapsed { get; }
    float Remaining { get; }

    bool IsPlaying { get; }
    bool IsCompleted { get; }
    bool IsKilled { get; }
    bool IsCompletedOrKilled { get; }

    void SetEasing(EasingDelegate easingFunction);

    void Reset(ResetMode mode);
    void Start();
    void Tick(float deltaTime);
    void Complete();
    void Kill();
}