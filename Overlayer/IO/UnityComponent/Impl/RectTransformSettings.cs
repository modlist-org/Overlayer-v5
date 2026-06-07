using Newtonsoft.Json.Linq;
using Overlayer.IO.Interface;
using UnityEngine;

namespace Overlayer.IO.UnityComponent.Impl;

public class RectTransformSettings : UnityComponentSettingsBase, ICopyable<RectTransformSettings> {
    public Vector2 AnchoredPosition = Vector2.zero;
    public Vector2 SizeDelta = new(200f, 200f);
    public Vector2 Pivot = new(0.5f, 0.5f);

    public Vector2 AnchorMin = Vector2.zero;
    public Vector2 AnchorMax = Vector2.one;
    public Vector2 OffsetMin = Vector2.zero;
    public Vector2 OffsetMax = Vector2.zero;

    public override bool ToUnity(GameObject target) {
        var com = target.GetComponent<RectTransform>();
        if(com == null) {
            return false;
        }

        com.anchorMin = AnchorMin;
        com.anchorMax = AnchorMax;
        com.pivot = Pivot;
        com.anchoredPosition = AnchoredPosition;
        com.sizeDelta = SizeDelta;
        com.offsetMin = OffsetMin;
        com.offsetMax = OffsetMax;

        return true;
    }

    public override bool FromUnity(GameObject source) {
        var com = source.GetComponent<RectTransform>();
        if(com == null) {
            return false;
        }

        AnchorMin = com.anchorMin;
        AnchorMax = com.anchorMax;
        Pivot = com.pivot;
        AnchoredPosition = com.anchoredPosition;
        SizeDelta = com.sizeDelta;
        OffsetMin = com.offsetMin;
        OffsetMax = com.offsetMax;

        return true;
    }

    public override JToken Serialize() {
        return new JObject {
            [nameof(AnchorMin)] = IOUtils.Write(AnchorMin),
            [nameof(AnchorMax)] = IOUtils.Write(AnchorMax),
            [nameof(AnchoredPosition)] = IOUtils.Write(AnchoredPosition),
            [nameof(SizeDelta)] = IOUtils.Write(SizeDelta),
            [nameof(Pivot)] = IOUtils.Write(Pivot),
            [nameof(OffsetMin)] = IOUtils.Write(OffsetMin),
            [nameof(OffsetMax)] = IOUtils.Write(OffsetMax)
        };
    }

    public override void Deserialize(JToken token) {
        AnchorMin = IOUtils.Read(token, nameof(AnchorMin), AnchorMin);
        AnchorMax = IOUtils.Read(token, nameof(AnchorMax), AnchorMax);
        AnchoredPosition = IOUtils.Read(token, nameof(AnchoredPosition), AnchoredPosition);
        SizeDelta = IOUtils.Read(token, nameof(SizeDelta), SizeDelta);
        Pivot = IOUtils.Read(token, nameof(Pivot), Pivot);
        OffsetMin = IOUtils.Read(token, nameof(OffsetMin), OffsetMin);
        OffsetMax = IOUtils.Read(token, nameof(OffsetMax), OffsetMax);
    }

    public RectTransformSettings Copy() {
        return new RectTransformSettings {
            AnchoredPosition = AnchoredPosition,
            SizeDelta = SizeDelta,
            Pivot = Pivot,
            AnchorMin = AnchorMin,
            AnchorMax = AnchorMax,
            OffsetMin = OffsetMin,
            OffsetMax = OffsetMax
        };
    }
}