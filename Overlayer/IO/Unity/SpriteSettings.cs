using Overlayer.IO.Interface;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Overlayer.IO.User;

namespace Overlayer.IO.Unity;

public class SpriteSettings : ISettingsFile {
    public string UserSpriteKey;
    public Rect Rect;
    public Vector2 Pivot;
    public float PixelsPerUnit = 100f;
    public Vector4 Border;

    public void FromUnity(Sprite sprite) {
        Rect = sprite.rect;
        Pivot = sprite.pivot;
        PixelsPerUnit = sprite.pixelsPerUnit;
        Border = sprite.border;
    }

    public Sprite ToUnity() {
        if(!UserResourceManager.T2D.TryGet(UserSpriteKey, out var texture)) {
            return null;
        }

        return Sprite.Create(
            texture,
            Rect,
            Pivot,
            PixelsPerUnit,
            0,
            SpriteMeshType.FullRect,
            Border,
            false
        );
    }

    public JToken Serialize() {
        return new JObject {
            [nameof(UserSpriteKey)] = UserSpriteKey,
            [nameof(Rect)] = IOUtils.Write(Rect),
            [nameof(Pivot)] = IOUtils.Write(Pivot),
            [nameof(PixelsPerUnit)] = PixelsPerUnit,
            [nameof(Border)] = IOUtils.Write(Border)
        };
    }

    public void Deserialize(JToken token) {
        UserSpriteKey = IOUtils.Read(token, nameof(UserSpriteKey), UserSpriteKey);
        Rect = IOUtils.Read(token, nameof(Rect), Rect);
        Pivot = IOUtils.Read(token, nameof(Pivot), Pivot);
        PixelsPerUnit = IOUtils.Read(token, nameof(PixelsPerUnit), PixelsPerUnit);
        Border = IOUtils.Read(token, nameof(Border), Border);
    }
}