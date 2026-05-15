using Newtonsoft.Json.Linq;

namespace Overlayer.IO;

public class Settings {
    public bool Active = true;
    public string Language = "en-US";
    public bool ShowOnStartup = false;

    public JToken Serialize() {
        JObject obj = new() {
            [nameof(Active)] = Active,
            [nameof(Language)] = Language,
            [nameof(ShowOnStartup)] = ShowOnStartup
        };

        return obj;
    }

    public void Deserialize(JToken token) {
        var defaults = new Settings();

        Active = token.Value<bool?>(nameof(Active)) ?? defaults.Active;
        Language = token.Value<string>(nameof(Language)) ?? defaults.Language;
        ShowOnStartup = token.Value<bool?>(nameof(ShowOnStartup)) ?? defaults.ShowOnStartup;
    }

    public static readonly string Path = System.IO.Path.Combine(Core.OverlayerPath, $"{nameof(Settings)}.json");

    public void Load() {
        if(File.Exists(Path)) {
            try {
                string json = File.ReadAllText(Path);
                JToken token = JToken.Parse(json);
                Deserialize(token);
            } catch(Exception e) {
                Core.Logger.Error($"Failed to load settings: {e}");
            }
        }
    }

    public void Save() {
        try {
            string json = Serialize().ToString();
            File.WriteAllText(Path, json);
        } catch(Exception e) {
            Core.Logger.Error($"Failed to save settings: {e}");
        }
    }
}
