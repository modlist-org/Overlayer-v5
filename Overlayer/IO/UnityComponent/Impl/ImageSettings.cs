using Newtonsoft.Json.Linq;
using Overlayer.IO.Unity;
using Overlayer.IO.User;
using UnityEngine;
using UnityEngine.UI;

namespace Overlayer.IO.UnityComponent.Impl;

public class ImageSettings : UnityComponentSettingsBase {
    public Color Color = Color.white;
    public SpriteSettings Sprite = new();
    public bool PreserveAspect = false;
    public Image.Type Type = Image.Type.Simple;
    public Image.FillMethod FillMethod = Image.FillMethod.Horizontal;
    public float FillAmount = 1f;

    public override void ToUnity(GameObject target) {
        var com = target.GetComponent<Image>();
        com.color = Color;
        com.sprite = Sprite.ToUnity();
        com.preserveAspect = PreserveAspect;
        com.type = Type;
        com.fillMethod = FillMethod;
        com.fillAmount = FillAmount;
    }

    public override void FromUnity(GameObject source) {
        var com = source.GetComponent<Image>();
        Color = com.color;
        if(com.sprite != null) {
            if(UserResourceManager.Spr.TryGetKey(com.sprite, out var spriteKey)) {
                Sprite.UserSpriteKey = spriteKey;
            }
            Sprite.FromUnity(com.sprite);
        }

        PreserveAspect = com.preserveAspect;
        Type = com.type;
        FillMethod = com.fillMethod;
        FillAmount = com.fillAmount;
    }

    public override JToken Serialize() {
        return new JObject {
            [nameof(Color)] = IOUtils.Write(Color),
            [nameof(Sprite)] = Sprite.Serialize(),
            [nameof(PreserveAspect)] = PreserveAspect,
            [nameof(Type)] = IOUtils.WriteEnum(Type),
            [nameof(FillMethod)] = IOUtils.WriteEnum(FillMethod),
            [nameof(FillAmount)] = FillAmount
        };
    }

    public override void Deserialize(JToken token) {
        Color = IOUtils.Read(token, nameof(Color), Color);
        var spriteToken = token[nameof(Sprite)];
        if(spriteToken != null) {
            Sprite.Deserialize(spriteToken);
        }

        PreserveAspect = IOUtils.Read(token, nameof(PreserveAspect), PreserveAspect);
        Type = IOUtils.ReadEnum(token, nameof(Type), Type);
        FillMethod = IOUtils.ReadEnum(token, nameof(FillMethod), FillMethod);
        FillAmount = IOUtils.Read(token, nameof(FillAmount), FillAmount);
    }
}