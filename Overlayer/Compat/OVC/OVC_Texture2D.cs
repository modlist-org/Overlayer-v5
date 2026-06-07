using UnityEngine;

#if IL2CPP
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
#endif

namespace Overlayer.Compat.OVC;

public static class OVC_Texture2D {
    public static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable = false) {
#if IL2CPP
        var il2cppData = new Il2CppStructArray<byte>(data.Length);
        il2cppData.AsSpan(data);

        return tex.LoadImage(il2cppData, markNonReadable);
#else
        return tex.LoadImage(data, markNonReadable);
#endif
    }
}
