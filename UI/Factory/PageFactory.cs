using System;
using Overlayer.Resource;
using Overlayer.UI.UISprites;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlayer.UI.Factory;

internal static class PageFactory {
    public static RectTransform CreatePages(GameObject panel) {
        GameObject pagesContainer = new("PagesContainer");
        pagesContainer.transform.SetParent(panel.transform, false);

        RectTransform container = pagesContainer.AddComponent<RectTransform>();
        container.anchorMin = new Vector2(0, 0);
        container.anchorMax = new Vector2(1, 1);
        container.pivot = new Vector2(0.5f, 0.5f);

        container.offsetMin = Vector2.zero;
        container.offsetMax = new Vector2(0, -60);

        for(int i = 0; i < Enum.GetValues(typeof(MenuState)).Length; i++) {
            MenuState type = (MenuState)i;

            GameObject obj = new($"Page{type}");
            obj.transform.SetParent(pagesContainer.transform, false);

            RectTransform rt = obj.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            CanvasGroup cg = obj.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;

            MenuFactory.Pages[type] = rt;
        }

        MenuFactory.Pages[MenuFactory.CurrentState].GetComponent<CanvasGroup>().alpha = 1f;
        MenuFactory.Pages[MenuFactory.CurrentState].GetComponent<CanvasGroup>().interactable = true;
        MenuFactory.Pages[MenuFactory.CurrentState].GetComponent<CanvasGroup>().blocksRaycasts = true;

        {
            var parent = MenuFactory.Pages[MenuState.Credits];

            {
                GameObject obj = new("Logo");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(200, 200);
                rect.anchoredPosition = new Vector2(0, 150);

                var img = obj.AddComponent<Image>();
                img.sprite = SpriteDatabase.Get(UISprite.OV5LogoOutline256);
                img.preserveAspect = true;
            }

            {
                GameObject obj = new("Title");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(800, 60);
                rect.anchoredPosition = new Vector2(0, -10);

                var tmp = obj.AddComponent<TextMeshProUGUI>();
                tmp.text = "Overlayer V5";
                tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
                tmp.fontSize = 38;
                tmp.color = Color.white;
                tmp.alignment = TextAlignmentOptions.Center;
            }

            {
                GameObject obj = new("Subtitle");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(800, 40);
                rect.anchoredPosition = new Vector2(0, -60);

                var tmp = obj.AddComponent<TextMeshProUGUI>();
                tmp.text = "Display everything as you wish.";
                tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
                tmp.fontSize = 20;
                tmp.color = new Color(1f, 1f, 1f, 0.45f);
                tmp.alignment = TextAlignmentOptions.Center;
            }

            {
                GameObject obj = new("Credits");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(900, 220);
                rect.anchoredPosition = new Vector2(0, -180);

                var tmp = obj.AddComponent<TextMeshProUGUI>();
                tmp.text =
                    "<color=#FFFFFF66>from modlist.org</color>\n" +
                    "<color=#FFFFFF88>Thank you for using Overlayer.</color>\n" +
                    "<size=12><color=#FFFFFF33>\nLicensed under GPLv3</color></size>";
                tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
                tmp.fontSize = 26;
                tmp.color = Color.white;
                tmp.lineSpacing = 18;
                tmp.alignment = TextAlignmentOptions.Center;
            }
        }

        {
            var parent = MenuFactory.Pages[MenuState.Settings];
            var languages = Core.Lang.GetLanguages();
            var nativeNames = Core.Lang.GetLanguageNativeNames();

            {
                GameObject obj = new("LanguageLabel");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(200, 50);
                rect.anchoredPosition = new Vector2(-150, 100);

                var tmp = obj.AddComponent<TextMeshProUGUI>();
                tmp.text = Core.Lang.Get("L_SETTING_LANGUAGE", "Language");
                tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
                tmp.fontSize = 24;
                tmp.color = Color.white;
                tmp.alignment = TextAlignmentOptions.Left;
            }

            {
                GameObject obj = new("LanguageSelector");
                obj.transform.SetParent(parent, false);

                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(300, 50);
                rect.anchoredPosition = new Vector2(100, 100);

                var tmp = obj.AddComponent<TextMeshProUGUI>();

                string currentNative = Core.Lang.GetForLanguage("0NATIVELANG", Core.Lang.Language, Core.Lang.Language);
                tmp.text = $"< {currentNative} >";

                tmp.font = ResourceManager.Get<TMP_FontAsset>(Asset.SUITRegular);
                tmp.fontSize = 24;
                tmp.color = new Color(1f, 1f, 1f, 0.8f);
                tmp.alignment = TextAlignmentOptions.Center;

                var btn = obj.AddComponent<Button>();
                btn.onClick.AddListener(() => {
                    int currentIndex = Array.IndexOf(languages, Core.Lang.Language);
                    int nextIndex = (currentIndex + 1) % languages.Length;

                    Core.Lang.Language = languages[nextIndex];

                    tmp.text = $"< {nativeNames[nextIndex]} >";
                });
            }
        }

        return container;
    }
}
