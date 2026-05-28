using Newtonsoft.Json.Linq;
using Overlayer.IO.Interface;

namespace Overlayer.IO.Overlay;

public sealed class CanvasSettings : ISettingsFile {
    private UnityEngine.Canvas a;

    public JToken Serialize() {
        return new JObject {
            
        };
    }

    public void Deserialize(JToken token) {
        
    }
}