using MelonLoader;
using MelonLoader.Utils;
using Overlayer;
using Overlayer.Async;
using Overlayer.Localization;
using Overlayer.Resource;
using Overlayer.UI;
using Overlayer.UI.UISprites;
using System.Collections;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(Core), Info.Name, Info.Version, Info.Author, Info.DownloadLink)]
[assembly: MelonGame("7th Beat Games", "A Dance of Fire and Ice")]

namespace Overlayer;

public class Core : MelonMod {
    public static readonly Version OverlayerVersion = new(Overlayer.Info.Version);
    internal static Assembly OverlayerAssembly = Assembly.GetExecutingAssembly();
    internal static MelonLogger.Instance Logger;
    internal static GameObject OverlayerObject;
    internal static Translator Lang;
    public static readonly string OverlayerPath = Path.Combine(
        MelonEnvironment.UserDataDirectory,
        "Overlayer"
    );

    private IEnumerator CreateOverlayerObject() {
        for(;;) {
            if(OverlayerObject == null) {
                OverlayerObject = new GameObject("Overlayer");
                UnityEngine.Object.DontDestroyOnLoad(OverlayerObject);

                if(OverlayerObject != null) {
                    Internal_Initialize();
                    yield break;
                }
            }

            yield return null;
        }
    }

    public override void OnInitializeMelon() {
        Logger = LoggerInstance;
        Lang = new Translator();
        Lang.SetLog(TranslatorLogLinker);
        _ = Lang.Load(Path.Combine(OverlayerPath, "Lang"));
        Initalize();
    }

    public void TranslatorLogLinker(string log) => Logger.Msg(log);

    public void Initalize() => MelonCoroutines.Start(CreateOverlayerObject());

    private void Internal_Initialize() {
        OverlayerObject.AddComponent<MainThread>();
        ResourceManager.Initialize();
        SpriteDatabase.Initialize();
        UICore.Initialize();
        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate() => UICore.HandleUpdate();

}