using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overlayer.IO.UnityComponent.Impl;

public class ShadowSettings : UnityComponentSettingsBase {
    public Vector2 EffectDistance = new(6, -6);
    public Color EffectColor = Color.black;
    public bool UseGraphicAlpha = true;

    public override void ToUnity(GameObject target) {
        var com = target.GetComponent<Shadow>();
        com.effectDistance = EffectDistance;
        com.effectColor = EffectColor;
        com.useGraphicAlpha = UseGraphicAlpha;
    }

    public override void FromUnity(GameObject source) {
        var com = source.GetComponent<Shadow>();
        EffectDistance = com.effectDistance;
        EffectColor = com.effectColor;
        UseGraphicAlpha = com.useGraphicAlpha;
    }

    public override JToken Serialize() {
        return new JObject {
            [nameof(EffectDistance)] = IOUtils.Write(EffectDistance),
            [nameof(EffectColor)] = IOUtils.Write(EffectColor),
            [nameof(UseGraphicAlpha)] = UseGraphicAlpha
        };
    }

    public override void Deserialize(JToken token) {
        EffectDistance = IOUtils.Read(token, nameof(EffectDistance), EffectDistance);
        EffectColor = IOUtils.Read(token, nameof(EffectColor), EffectColor);
        UseGraphicAlpha = IOUtils.Read(token, nameof(UseGraphicAlpha), UseGraphicAlpha);
    }
}