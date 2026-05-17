using HarmonyLib;
using Overlayer;
using Overlayer.Patch.Safe;
using System.Reflection;
using System.Reflection.Emit;

public class SP_ShowAutoJudgment : SafeConditionalPatch {
    public SP_ShowAutoJudgment() : base(nameof(SP_ShowAutoJudgment)) { }

    protected override bool ShouldApply() => Core.Config.ShowAutoplayJudgment;

    protected override MethodBase GetTargetMethod() => SafePatch.GetMethodSafe("scrPlayer", "Hit");

    protected override HarmonyMethod Transpiler() {
        return new HarmonyMethod(typeof(SP_ShowAutoJudgment)
            .GetMethod(nameof(TranspilerImpl), BindingFlags.Static | BindingFlags.NonPublic));
    }

    private static IEnumerable<CodeInstruction> TranspilerImpl(IEnumerable<CodeInstruction> instructions) {
        var codes = new List<CodeInstruction>(instructions);

        for(int i = 0; i < codes.Count - 1; i++) {
            bool isAutoField =
                codes[i].opcode == OpCodes.Ldarg_0 &&
                i + 1 < codes.Count &&
                codes[i + 1].opcode == OpCodes.Ldfld &&
                codes[i + 1].operand is FieldInfo f &&
                f.Name == "auto";

            bool isAutoGetter =
                codes[i].opcode == OpCodes.Ldarg_0 &&
                i + 1 < codes.Count &&
                codes[i + 1].opcode == OpCodes.Call &&
                codes[i + 1].operand is MethodInfo m &&
                m.Name.Contains("get_auto");

            if(isAutoField || isAutoGetter) {
                codes[i] = new CodeInstruction(OpCodes.Ldc_I4_0);
                codes[i + 1] = new CodeInstruction(OpCodes.Nop);
            }
        }

        return codes;
    }
}