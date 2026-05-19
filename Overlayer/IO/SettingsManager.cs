using Overlayer.Core;
using Newtonsoft.Json.Linq;

namespace Overlayer.IO;

public sealed class SettingsManager {
    public Settings Data { get; } = new();

    private readonly string path;

    private readonly object saveLock = new();

    private CancellationTokenSource saveCts;

    private bool saveScheduled;

    public SettingsManager(
        string path
    ) {
        this.path = path;
    }

    public void Load() {
        if (!File.Exists(path)) {
            return;
        }

        try {
            string json = File.ReadAllText(path);
            JToken token = JToken.Parse(json);
            SettingsSerializer.Deserialize(Data, token);
        } catch (Exception e) {
            MainCore.Logger.Err($"Failed to load settings: {e}");
        }
    }

    public void Save() {
        lock (saveLock) {
            try {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string json = SettingsSerializer.Serialize(Data).ToString();
                File.WriteAllText(path,json);
            }
            catch (Exception e) {
                MainCore.Logger.Err($"Failed to save settings: {e}");
            }
        }
    }

    public void RequestSave() {
        saveCts?.Cancel();

        saveCts = new CancellationTokenSource();

        var token = saveCts.Token;

        if (saveScheduled) {
            return;
        }

        saveScheduled = true;

        _ = Task.Run(async () => {
            try {
                await Task.Delay(500, token);

                if (token.IsCancellationRequested) {
                    return;
                }

                Save();
            } catch (OperationCanceledException) {
            } catch (Exception e) {
                MainCore.Logger.Err($"Failed to Request save settings: {e.ToString()}");
            } finally {
                saveScheduled = false;
            }
        });
    }
}