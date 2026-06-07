using GTweens.Delegates;

namespace GTweens.Extensions;

public static class ValidationExtensions {
    public static readonly ValidationDelegates.Validation AlwaysValid = () => true;
}