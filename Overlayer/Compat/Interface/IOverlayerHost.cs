namespace Overlayer.Compat.Interface;

public interface IOverlayerHost {
    IOverlayerLogger OverlayerLogger { get; }

    string OverlayerFilePath { get; }
    HarmonyLib.Harmony OverlayerHarmony { get; }
}