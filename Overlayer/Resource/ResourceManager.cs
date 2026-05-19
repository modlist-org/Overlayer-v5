using Overlayer.Core;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Overlayer.Resource;

public enum Asset {
    SUITRegular,
    SUITMedium,

    OV5LogoOutline256,
    Circle256,
    CircleHalf256,
    X128,
    Monitor128,
    Gear128,
    Image128,
    Text128,
    Book128,
    Star128,
    ToggleCircle128,
    CircleOutline256,
    Triangle128,
    Power128,
}

internal static class ResourceManager {
    private static bool initialized;

    private static readonly object initLock = new();

    private static readonly Dictionary<Asset, object>
        cache = [];

    public const string ResourcePath =
        "Overlayer.Resource.Embedded.";

    private static readonly (
        Asset key,
        string path,
        FilterMode filter
    )[] imageMap = [
        (Asset.OV5LogoOutline256, $"{ResourcePath}Image.OV5LogoOutline256.png", FilterMode.Bilinear),
        (Asset.Circle256, $"{ResourcePath}Image.Circle256.png", FilterMode.Bilinear),
        (Asset.CircleHalf256, $"{ResourcePath}Image.CircleHarf256.png", FilterMode.Bilinear),
        (Asset.X128, $"{ResourcePath}Image.X128.png", FilterMode.Bilinear),
        (Asset.Monitor128, $"{ResourcePath}Image.Monitor128.png", FilterMode.Bilinear),
        (Asset.Gear128, $"{ResourcePath}Image.Gear128.png", FilterMode.Bilinear),
        (Asset.Image128, $"{ResourcePath}Image.Image128.png", FilterMode.Bilinear),
        (Asset.Text128, $"{ResourcePath}Image.Text128.png", FilterMode.Bilinear),
        (Asset.Book128, $"{ResourcePath}Image.Book128.png", FilterMode.Bilinear),
        (Asset.Star128, $"{ResourcePath}Image.Star128.png", FilterMode.Bilinear),
        (Asset.ToggleCircle128, $"{ResourcePath}Image.ToggleCircle128.png", FilterMode.Bilinear),
        (Asset.CircleOutline256, $"{ResourcePath}Image.CircleOutline256.png", FilterMode.Bilinear),
        (Asset.Triangle128, $"{ResourcePath}Image.Triangle128.png", FilterMode.Bilinear),
        (Asset.Power128, $"{ResourcePath}Image.Power128.png", FilterMode.Bilinear)
    ];

    public static void Initialize() {
        lock(initLock) {
            if(initialized) {
                return;
            }

            initialized = true;

            string tempDir =
                MainCore.Paths.TempPath;

            Directory.CreateDirectory(
                tempDir
            );

            LoadFonts(tempDir);

            LoadTextures();
        }
    }

    public static T Get<T>(Asset key)
        where T : class {

        if(
            cache.TryGetValue(
                key,
                out object value
            )
        ) {
            return value as T;
        }

        return null;
    }

    public static void Dispose() {
        foreach(object item in cache.Values) {
            switch(item) {
                case Texture2D tex:
                    Object.Destroy(tex);
                    break;

                case TMP_FontAsset font:
                    Object.Destroy(font);
                    break;
            }
        }

        cache.Clear();

        initialized = false;
    }

    private static void LoadFonts(
        string tempDir
    ) {
        string regularPath = Path.Combine(
            tempDir,
            "SUIT-Regular.otf"
        );

        string mediumPath = Path.Combine(
            tempDir,
            "SUIT-Medium.otf"
        );

        WriteFontIfNeeded(
            regularPath,
            $"{ResourcePath}Font.SUIT-Regular.otf"
        );

        WriteFontIfNeeded(
            mediumPath,
            $"{ResourcePath}Font.SUIT-Medium.otf"
        );

        Font regularFont = new(regularPath);

        Font mediumFont = new(mediumPath);

        cache[Asset.SUITRegular] =
            TMP_FontAsset.CreateFontAsset(
                regularFont
            );

        cache[Asset.SUITMedium] =
            TMP_FontAsset.CreateFontAsset(
                mediumFont
            );
    }

    private static void LoadTextures() {
        foreach(
            (
                Asset key,
                string path,
                FilterMode filter
            ) in imageMap
        ) {
            byte[] data =
                ResourceLoader.Load(path);

            if(data == null) {
                MainCore.Logger.Err(
                    $"Failed to load resource: {path}"
                );

                continue;
            }

            Texture2D tex =
                ByteToTexture2D(data);

            if(tex == null) {
                MainCore.Logger.Err(
                    $"Failed to create texture: {path}"
                );

                continue;
            }

            tex.filterMode = filter;

            cache[key] = tex;
        }
    }

    private static void WriteFontIfNeeded(
        string outputPath,
        string resourcePath
    ) {
        if(File.Exists(outputPath)) {
            return;
        }

        byte[] data =
            ResourceLoader.Load(resourcePath);

        if(data == null) {
            MainCore.Logger.Err(
                $"Failed to load font resource: {resourcePath}"
            );

            return;
        }

        File.WriteAllBytes(
            outputPath,
            data
        );
    }

    private static Texture2D ByteToTexture2D(
        byte[] data
    ) {
        if(
            data == null ||
            data.Length == 0
        ) {
            return null;
        }

        Texture2D texture = new(
            2,
            2,
            TextureFormat.RGBA32,
            false,
            true
        );

        if(!texture.LoadImage(data)) {
            Object.Destroy(texture);

            return null;
        }

        return texture;
    }
}