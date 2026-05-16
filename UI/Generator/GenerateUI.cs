using DG.Tweening;
using Overlayer.Resource;
using Overlayer.UI.SpriteManage;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlayer.UI.Generator;

public static class GenerateUI {
    public static RectTransform Row(Transform parent, float height = 50f) {
        GameObject obj = new("Row");
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new(0f, 1f);
        rect.anchorMax = new(1f, 1f);
        rect.pivot = new(0.5f, 1f);
        rect.offsetMin = new(0f, -height);
        rect.offsetMax = Vector2.zero;

        return rect;
    }

    public static RectTransform Toggle(Transform parent, bool defaultValue, bool value, Action<bool> onChanged, string text) {
        RectTransform rect = BackGround();
        rect.SetParent(parent, false);

        TextMeshProUGUI tmp = AddText(rect);
        tmp.text = text;

        GameObject change = AddSmallChangedCircle(rect);
        var changeImg = change.GetComponent<Image>();

        GameObject toggleCircle = new("ToggleCircle");
        toggleCircle.transform.SetParent(rect, false);

        RectTransform circleRect = toggleCircle.AddComponent<RectTransform>();
        circleRect.anchorMin = new(1f, 0.5f);
        circleRect.anchorMax = new(1f, 0.5f);
        circleRect.pivot = new(0.5f, 0.5f);
        circleRect.anchoredPosition = new(-23f, 0f);
        circleRect.sizeDelta = new(26f, 26f);

        Image circleImage = toggleCircle.AddComponent<Image>();

        Sequence circleSeq = null;
        Sequence changeSeq = null;

        void UpdateVisual() {
            circleImage.sprite = SpriteDatabase.Get(
                value ? UISprite.Circle256 : UISprite.ToggleCircle128
            );

            circleSeq?.Kill();
            circleRect.sizeDelta = new(30f, 30f);
            circleSeq = DOTween.Sequence()
                .Join(
                    DOTween.To(
                        () => circleRect.sizeDelta.x,
                        x => circleRect.sizeDelta = new(x, x),
                        26f,
                        0.3f
                    ).SetEase(Ease.OutQuad)
                )
                .Join(
                    circleImage.DOColor(
                        value
                            ? new(0.569f, 0.604f, 1f, 1f)
                            : new(0.384f, 0.4f, 0.588f, 1f),
                        0.15f
                    ).SetEase(Ease.OutQuad)
                );
            changeSeq?.Kill();

            float target = defaultValue != value ? 1f : 0f;
            changeSeq = DOTween.Sequence().Append(
                DOTween.To(
                    () => changeImg.color.a,
                    x => {
                        Color c = changeImg.color;
                        c.a = x;
                        changeImg.color = c;
                    },
                    target,
                    0.2f
                ).SetEase(Ease.OutSine)
            );
        }

        UpdateVisual();

        AddButton(rect.gameObject, (isRight) => {
            if(isRight) {
                if(Core.Config.RightClickToDefault && value != defaultValue) {
                    value = defaultValue;
                    UpdateVisual();
                    onChanged?.Invoke(value);
                }
            } else {
                value = !value;
                UpdateVisual();
                onChanged?.Invoke(value);
            }
        });

        return rect;
    }

    public static void AddEvent(EventTriggerType type, Action<PointerEventData> cb, EventTrigger trigger) {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(e => cb((PointerEventData)e));
        trigger.triggers.Add(entry);
    }

    public enum BackGroundType {
        Main,
        Sub,
        Full
    }

    public static RectTransform BackGround(BackGroundType type = BackGroundType.Main) {
        GameObject obj = new("Bg");
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new(0f, 0f);
        rect.anchorMax = new(1f, 1f);
        rect.pivot = new(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        switch(type) {
            case BackGroundType.Main: {
                rect.offsetMax = new(-300f, 0f);
                break;
            }

            case BackGroundType.Sub: {
                rect.anchorMin = new(1f, 0f);
                rect.sizeDelta = new(290f, 0f);
                rect.offsetMax = new(-10f, 0f);
                break;
            }

            case BackGroundType.Full: {
                rect.pivot = new(0.5f, 0.5f);
                break;
            }
        }

        Image img = obj.AddComponent<Image>();
        img.color = new(0.235f, 0.227f, 0.294f, 1f);
        img.sprite = SpriteDatabase.Get(UISliceSprite.Circle256P2048);
        img.type = Image.Type.Sliced;

        return rect;
    }

    public static void AddButton(GameObject obj, Action<bool> onClick) {
        EventTrigger trigger = obj.AddComponent<EventTrigger>();

        Sequence hoverSeq = null;

        GameObject hover = new("Hover");
        hover.transform.SetParent(obj.transform, false);
        hover.transform.SetAsFirstSibling();

        RectTransform hoverRect = hover.AddComponent<RectTransform>();
        hoverRect.anchorMin = Vector2.zero;
        hoverRect.anchorMax = Vector2.one;
        hoverRect.pivot = new(0.5f, 0.5f);
        hoverRect.offsetMin = Vector2.zero;
        hoverRect.offsetMax = Vector2.zero;

        Image hoverImage = hover.AddComponent<Image>();
        hoverImage.sprite = SpriteDatabase.Get(UISliceSprite.CircleOutline256P2048);
        hoverImage.type = Image.Type.Sliced;
        hoverImage.color = new(0.569f, 0.604f, 1f, 0f);

        AddEvent(EventTriggerType.PointerClick, (e) => {
            bool isRight = e.button == PointerEventData.InputButton.Right;
            onClick?.Invoke(isRight);
        }, trigger);

        AddEvent(EventTriggerType.PointerEnter, (e) => {
            hoverSeq?.Kill();

            hoverSeq = DOTween.Sequence().Append(
                DOTween.To(
                    () => hoverImage.color.a,
                    x => {
                        Color c = hoverImage.color;
                        c.a = x;
                        hoverImage.color = c;
                    },
                    1f,
                    0.1f
                ).SetEase(Ease.OutSine)
            );
        }, trigger);

        AddEvent(EventTriggerType.PointerExit, (e) => {
            hoverSeq?.Kill();

            hoverSeq = DOTween.Sequence().Append(
                DOTween.To(
                    () => hoverImage.color.a,
                    x => {
                        Color c = hoverImage.color;
                        c.a = x;
                        hoverImage.color = c;
                    },
                    0f,
                    0.1f
                ).SetEase(Ease.OutSine)
            );
        }, trigger);
    }

    public static TextMeshProUGUI AddText(RectTransform parent) => CreateText(parent, 24f, FontStyles.Normal);

    public static TextMeshProUGUI AddTextH1(RectTransform parent) => CreateText(parent, 32f, FontStyles.Bold);

    private static TextMeshProUGUI CreateText(RectTransform parent, float size, FontStyles style) {
        GameObject obj = new("Text");
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new(0f, 0f);
        rect.anchorMax = new(1f, 1f);
        rect.offsetMin = new(16f, 0f);
        rect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
        tmp.fontSize = size;
        tmp.fontStyle = style;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.verticalAlignment = VerticalAlignmentOptions.Middle;

        return tmp;
    }

    public static GameObject AddSmallChangedCircle(RectTransform parent) {
        GameObject obj = new("Changed");
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(6f, -6f);
        rect.sizeDelta = new Vector2(8f, 8f);

        Image img = obj.AddComponent<Image>();
        img.sprite = SpriteDatabase.Get(UISprite.Circle256);
        img.color = new Color(0.396f, 0.416f, 0.651f, 1f);

        return obj;
    }

    public static Transform AddToolTip(this Transform parent, string key, string def) {
        EventTrigger trigger = parent.gameObject.GetComponent<EventTrigger>();
        if(trigger == null) {
            trigger = parent.gameObject.AddComponent<EventTrigger>();
        }

        AddEvent(EventTriggerType.PointerEnter, (e) => {
            Tooltip.Show(Core.Tr.Get(key, def));
        }, trigger);

        AddEvent(EventTriggerType.PointerExit, (e) => {
            Tooltip.Hide();
        }, trigger);

        return parent;
    }
}
