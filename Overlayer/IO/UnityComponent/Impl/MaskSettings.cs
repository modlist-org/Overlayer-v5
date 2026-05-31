using Newtonsoft.Json.Linq;
using Overlayer.IO.Unity;
using UnityEngine;
using UnityEngine.UI;
namespace Overlayer.IO.UnityComponent.Impl;

public class MaskSettings : UnityComponentSettingsBase {
    public bool ShowMaskGraphic = true;

    public override void ToUnity(GameObject target) {
        var com = target.GetComponent<Mask>();
        com.showMaskGraphic = ShowMaskGraphic;
    }

    public override void FromUnity(GameObject source) {
        var com = source.GetComponent<Mask>();
        ShowMaskGraphic = com.showMaskGraphic;
    }

    public override JToken Serialize() {
        return new JObject {
            [nameof(ShowMaskGraphic)] = ShowMaskGraphic,
        };
    }

    public override void Deserialize(JToken token) {
        ShowMaskGraphic = IOUtils.Read(token, nameof(ShowMaskGraphic), ShowMaskGraphic);
    }
}