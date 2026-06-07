using System.Drawing;

namespace GTweens.Extensions;

public static class SystemColorExtensions {
    public static Color FromRgba(float r, float g, float b, float a) {
        return Color.FromArgb(
            (int)(a * 255f),
            (int)(r * 255f),
            (int)(g * 255f),
            (int)(b * 255f)
        );
    }
}