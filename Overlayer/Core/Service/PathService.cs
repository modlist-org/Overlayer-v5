using System.IO;

namespace Overlayer.Core.Service;

public sealed class PathService(string rootPath, string dllPath) {
    public string RootPath { get; } = rootPath;
    public string DLLPath { get; } = dllPath;

    public string ConfigPath => Path.Combine(RootPath, "Settings.json");
    public string LangPath => Path.Combine(RootPath, "Lang");
    public string TempPath => Path.Combine(RootPath, "Temp");

    public void Initialize() {
        Directory.CreateDirectory(RootPath);
        Directory.CreateDirectory(LangPath);
        Directory.CreateDirectory(TempPath);
    }
}