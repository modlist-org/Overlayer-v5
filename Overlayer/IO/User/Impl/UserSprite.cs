using Newtonsoft.Json.Linq;
using Overlayer.Core;
using Overlayer.IO.Interface;
using Overlayer.IO.Unity;
using UnityEngine;

namespace Overlayer.IO.User.Impl;

public class UserSprite : UserResourceBase<Sprite>, ISettingsFile {
    public enum Result {
        Success,
        KeyAlreadyExists,
        NotFound,
        Failed,
    }

    public Result Load(
        string key,
        string textureKey,
        Rect rect,
        Vector2 pivot,
        float pixelsPerUnit,
        Vector4 border,
        out Sprite sprite
    ) {
        sprite = null;

        try {
            if(Cache.ContainsKey(key)) {
                sprite = Cache[key].value;
                return Result.KeyAlreadyExists;
            }

            if(!UserResourceManager.T2D.TryGet(textureKey, out var value)) {
                return Result.NotFound;
            }

            var spr = Sprite.Create(
                value,
                rect,
                pivot,
                pixelsPerUnit,
                0,
                SpriteMeshType.FullRect,
                border,
                false
            );

            Cache[key] = (textureKey, spr);
            sprite = spr;

            return Result.Success;
        } catch(Exception e) {
            MainCore.Logger.Err($"[{nameof(UserSprite)}] Sprite load failed: {e}");
            return Result.Failed;
        }
    }

    public JToken Serialize() {
        var obj = new JObject();

        foreach(var (key, (textureKey, sprite)) in Cache) {
            var settings = new SpriteSettings {
                UserSpriteKey = textureKey
            };

            settings.FromUnity(sprite);
            obj[key] = settings.Serialize();
        }

        return obj;
    }

    public void Deserialize(JToken token) {
        if(token is not JObject obj) {
            MainCore.Logger.Wrn(
                $"[{nameof(UserSprite)}] Deserialize failed: token is not JObject"
            );
            return;
        }

        foreach(var property in obj.Properties()) {
            var settings = new SpriteSettings();
            settings.Deserialize(property.Value);

            var result = Load(
                property.Name,
                settings.UserSpriteKey,
                settings.Rect,
                settings.Pivot,
                settings.PixelsPerUnit,
                settings.Border,
                out _
            );

            if(result != Result.Success) {
                MainCore.Logger.Wrn(
                    $"[{nameof(UserSprite)}] {result} {{ \"{property.Name}\": \"{settings.UserSpriteKey}\" }}"
                );
            }
        }
    }


    public override void Dispose() {
        foreach(var (_, value) in Cache.Values) {
            UnityEngine.Object.Destroy(value);
        }

        Cache.Clear();
    }
}