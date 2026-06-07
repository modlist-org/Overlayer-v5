using Overlayer.Tag.Core;

namespace Overlayer.Tag.Impl;

public class Color {
    [Tag(Desc = "\"Early!!\" Hex Color")]
    public const string TEHex = "FF0000";

    [Tag(Desc = "\"Early\" Hex Color")]
    public const string VEHex = "FF6F4E";

    [Tag(Desc = "\"EPerfect\" Hex Color")]
    public const string EPHex = "FCFF4D";

    [Tag(Desc = "\"Perfect!\" Hex Color")]
    public const string PHex = "60FF4E";

    [Tag(Desc = "\"LPerfect\" Hex Color")]
    public const string LPHex = "FCFF4D";

    [Tag(Desc = "\"Late\" Hex Color")]
    public const string VLHex = "FF6F4E";

    [Tag(Desc = "\"Late!!\" Hex Color")]
    public const string TLHex = "FF0000";

    [Tag(Desc = "\"Multipress\" Hex Color")]
    public const string MultipressHex = "00FFED";

    [Tag(Desc = "\"Miss...\" Hex Color")]
    public const string MissHex = "D958FF";

    [Tag(Desc = "\"Overload...\" Hex Color")]
    public const string OverloadHex = "D958FF";
}
