using Newtonsoft.Json.Linq;
using Overlayer.IO.Interface;
using UnityEngine;

namespace Overlayer.IO.UnityComponent;

public class RectTransformSettings : ISettingsFile
{
    public bool IsWorld;

    // SCREEN SPACE (UI)
    public Vector2? AnchorMin;
    public Vector2? AnchorMax;
    public Vector2? AnchoredPosition;
    public Vector2? SizeDelta;
    public Vector2? Pivot;

    public Vector2? OffsetMin;
    public Vector2? OffsetMax;

    // WORLD SPACE
    public Vector3? LocalPosition;
    public Vector3? LocalEulerAngles;
    public Vector3? LocalScale;

    public JToken Serialize()
    {
        var obj = new JObject
        {
            [nameof(IsWorld)] = IsWorld
        };

        void Add(string key, object? value)
        {
            if (value != null)
            {
                obj[key] = JToken.FromObject(value);
            }
        }

        if (IsWorld)
        {
            Add(nameof(LocalPosition), LocalPosition);
            Add(nameof(LocalEulerAngles), LocalEulerAngles);
            Add(nameof(LocalScale), LocalScale);
        }
        else
        {
            Add(nameof(AnchorMin), AnchorMin);
            Add(nameof(AnchorMax), AnchorMax);
            Add(nameof(AnchoredPosition), AnchoredPosition);
            Add(nameof(SizeDelta), SizeDelta);
            Add(nameof(Pivot), Pivot);
            Add(nameof(OffsetMin), OffsetMin);
            Add(nameof(OffsetMax), OffsetMax);
        }

        return obj;
    }

    public void Deserialize(JToken token)
    {
        IsWorld = IOUtils.Read(token, nameof(IsWorld), IsWorld);

        if (IsWorld)
        {
            LocalPosition = IOUtils.Read(token, nameof(LocalPosition), LocalPosition);
            LocalEulerAngles = IOUtils.Read(token, nameof(LocalEulerAngles), LocalEulerAngles);
            LocalScale = IOUtils.Read(token, nameof(LocalScale), LocalScale);
        }
        else
        {
            AnchorMin = IOUtils.Read(token, nameof(AnchorMin), AnchorMin);
            AnchorMax = IOUtils.Read(token, nameof(AnchorMax), AnchorMax);
            AnchoredPosition = IOUtils.Read(token, nameof(AnchoredPosition), AnchoredPosition);
            SizeDelta = IOUtils.Read(token, nameof(SizeDelta), SizeDelta);
            Pivot = IOUtils.Read(token, nameof(Pivot), Pivot);
            OffsetMin = IOUtils.Read(token, nameof(OffsetMin), OffsetMin);
            OffsetMax = IOUtils.Read(token, nameof(OffsetMax), OffsetMax);
        }
    }
}