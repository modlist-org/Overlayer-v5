using Overlayer.IO;
using Overlayer.UI.Generator;
using UnityEngine;
using UnityEngine.UI;

namespace Overlayer.UI.Factory.Page;

internal static class PageSettings {
    public static void Create(RectTransform parent) {
        GameObject pad = new("Pad");
        pad.transform.SetParent(parent, false);

        RectTransform rect = pad.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new(0.5f, 0.5f);
        rect.offsetMin = new(18f, 18f);
        rect.offsetMax = new(-18f, -18f);

        ScrollRect scroll = pad.AddComponent<ScrollRect>();
        scroll.horizontal = false;

        GameObject viewport = new("Viewport");
        viewport.transform.SetParent(pad.transform, false);

        RectTransform viewportRect = viewport.AddComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = Vector2.zero;
        viewportRect.offsetMax = Vector2.zero;

        viewport.AddComponent<RectMask2D>();

        GameObject content = new("Content");
        content.transform.SetParent(viewport.transform, false);

        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchorMin = new(0f, 1f);
        contentRect.anchorMax = new(1f, 1f);
        contentRect.pivot = new(0.5f, 1f);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;

        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 12f;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scroll.viewport = viewportRect;
        scroll.content = contentRect;

        var defSet = new Settings();
        GenerateUI.Toggle(GenerateUI.Row(content.transform), defSet.ShowOnStartup, Core.Config.ShowOnStartup, (toggle) => {
            Core.Config.ShowOnStartup = toggle;
            Core.Config.RequestSave();
        }, "Show Overlayer Panel at Startup").AddToolTip("KEY", "Show Overlayer Panel at Startup");
        GenerateUI.Toggle(GenerateUI.Row(content.transform), defSet.RightClickToDefault, Core.Config.RightClickToDefault, (toggle) => {
            Core.Config.RightClickToDefault = toggle;
            Core.Config.RequestSave();
        }, "Right-click to set as default").AddToolTip("KEY", "Right-click to set as default");
    }
}