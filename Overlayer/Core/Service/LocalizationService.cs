using Overlayer.Compat;
using Overlayer.Compat.Interface;
using Overlayer.IO;
using Overlayer.Localization;

namespace Overlayer.Core.Service;

public sealed class LocalizationService(
    string langPath,
    Settings config,
    OverlayerLogger logger
) : IRuntimeService {

    public Translator Translator { get; } = new();

    public void Initialize() {
        Translator.Language =
            config.Language;

        Translator.SetLog(
            logger.Msg
        );

        _ = Translator.Load(langPath);
    }

    public void Dispose() { }
}