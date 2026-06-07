namespace GTweens.Easings;

public static class PresetEasingDelegateFactory {
    const float C1 = 1.70158f;
    const float C2 = C1 * 1.525f;
    const float C3 = C1 + 1;
    const float C4 = 2 * (float)Math.PI / 3;
    const float C5 = 2 * (float)Math.PI / 4.5f;

    const float N1 = 7.5625f;
    const float D1 = 2.75f;

    public static EasingDelegate GetEaseDelegate(Easing easing) {
        EasingDelegate result = easing switch {
            Easing.InSine => InSine,
            Easing.OutSine => OutSine,
            Easing.InOutSine => InOutSine,
            Easing.InQuad => InQuad,
            Easing.OutQuad => OutQuad,
            Easing.InOutQuad => InOutQuad,
            Easing.InCubic => InCubic,
            Easing.OutCubic => OutCubic,
            Easing.InOutCubic => InOutCubic,
            Easing.InQuart => InQuart,
            Easing.OutQuart => OutQuart,
            Easing.InOutQuart => InOutQuart,
            Easing.InQuint => InQuint,
            Easing.OutQuint => OutQuint,
            Easing.InOutQuint => InOutQuint,
            Easing.InExpo => InExpo,
            Easing.OutExpo => OutExpo,
            Easing.InOutExpo => InOutExpo,
            Easing.InCirc => InCirc,
            Easing.OutCirc => OutCirc,
            Easing.InOutCirc => InOutCirc,
            Easing.InBack => InBack,
            Easing.OutBack => OutBack,
            Easing.InOutBack => InOutBack,
            Easing.InElastic => InElastic,
            Easing.OutElastic => OutElastic,
            Easing.InOutElastic => InOutElastic,
            Easing.InBounce => InBounce,
            Easing.OutBounce => OutBounce,
            Easing.InOutBounce => InOutBounce,
            _ => Linear,
        };
        return result;
    }

    static float Linear(float a, float b, float t) => Lerp(a, b, t);

    static float InSine(float a, float b, float t) => Lerp(a, b, 1 - (float)Math.Cos(t * (float)Math.PI / 2f));

    static float OutSine(float a, float b, float t) => Lerp(a, b, (float)Math.Sin(t * (float)Math.PI / 2f));

    static float InOutSine(float a, float b, float t) => Lerp(a, b, -((float)Math.Cos(t * (float)Math.PI) - 1) / 2f);

    static float InQuad(float a, float b, float t) => Lerp(a, b, t * t);

    static float OutQuad(float a, float b, float t) => Lerp(a, b, 1 - ((1 - t) * (1 - t)));

    static float InOutQuad(float a, float b, float t) => Lerp(a, b, t < 0.5f ? 2 * t * t : 1 - ((float)Math.Pow((-2 * t) + 2, 2) / 2));

    static float InCubic(float a, float b, float t) => Lerp(a, b, t * t * t);

    static float OutCubic(float a, float b, float t) => Lerp(a, b, 1 - (float)Math.Pow(1 - t, 3));

    static float InOutCubic(float a, float b, float t) => Lerp(a, b, t < 0.5f ? 4 * t * t * t : 1 - ((float)Math.Pow((-2 * t) + 2, 3) / 2));

    static float InQuart(float a, float b, float t) => Lerp(a, b, t * t * t * t);

    static float OutQuart(float a, float b, float t) => Lerp(a, b, 1 - (float)Math.Pow(1 - t, 4));

    static float InOutQuart(float a, float b, float t) => Lerp(a, b, t < 0.5f ? 8 * t * t * t * t : 1 - ((float)Math.Pow((-2 * t) + 2, 4) / 2));

    static float InQuint(float a, float b, float t) => Lerp(a, b, t * t * t * t * t);

    static float OutQuint(float a, float b, float t) => Lerp(a, b, 1 - (float)Math.Pow(1 - t, 5));

    static float InOutQuint(float a, float b, float t) => Lerp(a, b, t < 0.5f ? 16 * t * t * t * t * t : 1 - ((float)Math.Pow((-2 * t) + 2, 5) / 2));

    static float InExpo(float a, float b, float t) => Lerp(a, b, t == 0 ? 0 : (float)Math.Pow(2, (10 * t) - 10));

    static float OutExpo(float a, float b, float t) => Lerp(a, b, t == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * t));

    static float InOutExpo(float a, float b, float t) => Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? (float)Math.Pow(2, (20 * t) - 10) / 2 : (2 - (float)Math.Pow(2, (-20 * t) + 10)) / 2);

    static float InCirc(float a, float b, float t) => Lerp(a, b, 1 - (float)Math.Sqrt(1 - (float)Math.Pow(t, 2)));

    static float OutCirc(float a, float b, float t) => Lerp(a, b, (float)Math.Sqrt(1 - (float)Math.Pow(t - 1, 2)));

    static float InOutCirc(float a, float b, float t) => Lerp(a, b, t < 0.5f ? (1 - (float)Math.Sqrt(1 - (float)Math.Pow(2 * t, 2))) / 2 : (float)(Math.Sqrt(1 - Math.Pow((-2 * t) + 2, 2)) + 1) / 2);

    static float InBack(float a, float b, float t) => Lerp(a, b, (C3 * t * t * t) - (C1 * t * t));

    static float OutBack(float a, float b, float t) => Lerp(a, b, 1 + (C3 * (float)Math.Pow(t - 1, 3)) + (C1 * (float)Math.Pow(t - 1, 2)));

    static float InOutBack(float a, float b, float t) => Lerp(a, b, t < 0.5f ? (float)Math.Pow(2 * t, 2) * (((C2 + 1) * 2 * t) - C2) / 2 : (((float)Math.Pow((2 * t) - 2, 2) * (((C2 + 1) * ((t * 2) - 2)) + C2)) + 2) / 2);

    static float InElastic(float a, float b, float t) => Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : -(float)Math.Pow(2, (10 * t) - 10) * (float)Math.Sin(((t * 10) - 10.75f) * C4));

    static float OutElastic(float a, float b, float t) => Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : ((float)Math.Pow(2, -10 * t) * (float)Math.Sin(((t * 10) - 0.75f) * C4)) + 1);

    static float InOutElastic(float a, float b, float t) => Lerp(a, b, t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? -((float)Math.Pow(2, (20 * t) - 10) * (float)Math.Sin(((20 * t) - 11.125f) * C5)) / 2 : (float)Math.Pow(2, (-20 * t) + 10) * (float)Math.Sin(((20 * t) - 11.125f) * C5) / +1);

    static float InBounce(float a, float b, float t) => Lerp(a, b, 1 - RawOutBounce(1 - t));

    static float OutBounce(float a, float b, float t) => Lerp(a, b, RawOutBounce(t));

    static float InOutBounce(float a, float b, float t) => Lerp(a, b, t < 0.5f ? (1 - RawOutBounce(1 - (2 * t))) / 2 : (1 + RawOutBounce((2 * t) - 1)) / 2);

    static float RawOutBounce(float t) {
        if(t < 1 / D1) {
            return N1 * t * t;
        }

        if(t < 2 / D1) {
            return (N1 * (t -= 1.5f / D1) * t) + 0.75f;
        }

        if(t < 2.5f / D1) {
            return (N1 * (t -= 2.25f / D1) * t) + 0.9375f;
        }

        return (N1 * (t -= 2.625f / D1) * t) + 0.984375f;
    }

    static float Lerp(float a, float b, float t) => a + ((b - a) * t);
}