using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlayer.UI.Objects.Impl;

public class UISlider : UIObject {
    public float DefaultValue { get; }
    public float Min;
    public float Max;
    public float Value { get; private set; }

    private string _format;
    public string Format {
        get => _format;
        set {
            _format = value;
            UpdateValueText();
        }
    }

    public Action<float> OnChanged;
    public Action<float> OnComplete;

    public Func<float, float> Filter;

    public RectTransform FillRect { get; }
    public Image FillImage { get; }

    public TextMeshProUGUI Label { get; }
    public TextMeshProUGUI ValueText { get; }

    public Image ChangedImage { get; }
    public Image ChangedImageUp { get; }

    private Sequence fillSeq;
    private Sequence changeSeq;

    public UISlider(
        string id,
        RectTransform rect,
        RectTransform fillRect,
        Image fillImage,
        TextMeshProUGUI label,
        TextMeshProUGUI valueText,
        Image changedImage,
        Image changedImageUp,
        float defaultValue,
        float min,
        float max,
        float value,
        Func<float, float> filter,
        Action<float> onChanged,
        Action<float> onComplete,
        string format = "0.##"
    ) : base(id, rect) {
        FillRect = fillRect;
        FillImage = fillImage;

        Label = label;
        ValueText = valueText;

        ChangedImage = changedImage;
        ChangedImageUp = changedImageUp;

        DefaultValue = defaultValue;
        Min = min;
        Max = max;

        OnChanged = onChanged;
        OnComplete = onComplete;

        Format = format;
        Filter = filter;

        Value = ApplyFilter(value);
        Value = Mathf.Clamp(Value, Min, Max);

        UpdateVisual(false);
    }

    public void Set(float value, bool invoke = true) {
        value = ApplyFilter(value);

        Value = Mathf.Clamp(value, Min, Max);

        if(invoke) {
            OnChanged?.Invoke(Value);
        }

        UpdateVisual();
    }

    public void SetOnlyValue(float value) {
        Value = Mathf.Clamp(ApplyFilter(value), Min, Max);
        UpdateVisual(false);
    }

    public float Normalize() => Mathf.InverseLerp(Min, Max, Value);

    public void SetNormalized(float t, bool invoke = true)
        => Set(Mathf.Lerp(Min, Max, t), invoke);

    private float ApplyFilter(float v) {
        if(Filter == null) {
            return v;
        }

        return Filter(v);
    }

    private void UpdateValueText() => ValueText?.text = Value.ToString(Format);

    public void UpdateVisual(bool animate = true) {
        float t = Normalize();

        UpdateValueText();

        fillSeq?.Kill();

        if(animate) {
            fillSeq = DOTween.Sequence().SetUpdate(true).Join(
                DOTween.To(
                    () => FillRect.anchorMax.x,
                    x => {
                        Vector2 max = FillRect.anchorMax;
                        max.x = x;
                        FillRect.anchorMax = max;
                    },
                    t,
                    0.6f
                )
                .SetEase(Ease.OutExpo)
            );
        } else {
            Vector2 max = FillRect.anchorMax;
            max.x = t;
            FillRect.anchorMax = max;
        }

        FillImage.color = UIColors.ObjectActive;

        changeSeq?.Kill();
        float changeAlpha = DefaultValue == Value ? 0f : 1f;
        changeSeq = DOTween.Sequence().SetUpdate(true)
            .Join(DOTween.To(
                () => ChangedImage.color.a,
                x => {
                    Color c = ChangedImage.color;
                    c.a = x;
                    ChangedImage.color = c;
                },
                changeAlpha,
                0.2f
            ).SetEase(Ease.OutSine))
            .Join(DOTween.To(
                () => ChangedImageUp.color.a,
                x => {
                    Color c = ChangedImageUp.color;
                    c.a = x;
                    ChangedImageUp.color = c;
                },
                changeAlpha,
                0.2f
            ).SetEase(Ease.OutSine)
        );
    }

    public void OnDrag(float normalizedValue) {
        SetNormalized(normalizedValue, true);
    }
}