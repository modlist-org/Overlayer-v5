using Newtonsoft.Json.Linq;

namespace Overlayer.IO;

internal static class SettingsSerializer {
    private static readonly Settings defaults = new();

    public static JToken Serialize(
        Settings settings
    ) {
        return new JObject {
            [nameof(Settings.Active)] = settings.Active,
            [nameof(Settings.Language)] = settings.Language,
            [nameof(Settings.ShowOnStartup)] = settings.ShowOnStartup,
            [nameof(Settings.Tooltip)] = settings.Tooltip,
            [nameof(Settings.MiddleClickToDefault)] = settings.MiddleClickToDefault,
            [nameof(Settings.UIScale)] = settings.UIScale,
            [nameof(Settings.ShowAutoplayJudgment)] = settings.ShowAutoplayJudgment
        };
    }

    public static void Deserialize(
        Settings settings,
        JToken token
    ) {
        settings.Active = Read(token, nameof(Settings.Active), defaults.Active);
        settings.Language = Read(token, nameof(Settings.Language), defaults.Language);
        settings.ShowOnStartup = Read(token, nameof(Settings.ShowOnStartup), defaults.ShowOnStartup);
        settings.Tooltip = Read(token, nameof(Settings.Tooltip), defaults.Tooltip);
        settings.MiddleClickToDefault = Read(token, nameof(Settings.MiddleClickToDefault), defaults.MiddleClickToDefault);
        settings.UIScale = Read(token, nameof(Settings.UIScale), defaults.UIScale);
        settings.ShowAutoplayJudgment = Read(token, nameof(Settings.ShowAutoplayJudgment), defaults.ShowAutoplayJudgment);
    }

    private static T Read<T>(
        JToken token,
        string key,
        T fallback
    ) {
        JToken value = token[key];

        if(value == null) {
            return fallback;
        }

        try {
            return value.Value<T>();
        } catch {
            return fallback;
        }
    }
}