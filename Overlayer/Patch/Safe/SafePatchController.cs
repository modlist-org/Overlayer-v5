namespace Overlayer.Patch.Safe;

public static class SafePatchController {
    private static readonly SafeConditionalPatch[] patches = [
        new SP_ShowAutoJudgment()
    ];

    public static T Get<T>() where T : SafeConditionalPatch {
        foreach(var patch in patches) {
            if(patch is T typed) {
                return typed;
            }
        }

        return null;
    }

    public static void ApplyAll() {
        foreach(var patch in patches) {
            patch.Apply();
        }
    }

    public static void UnloadAll() {
        foreach(var patch in patches) {
            patch.Remove();
        }
    }
}