using Newtonsoft.Json.Linq;

namespace Overlayer.IO;

public static class IOUtils {
    public static T Read<T>(JToken token, string key, T fallback) {
        var value = token[key];

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