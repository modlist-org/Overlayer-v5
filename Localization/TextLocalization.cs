using UnityEngine;
using TMPro;

namespace Overlayer.Localization;

public class LocalizedText : MonoBehaviour {
    public string Key;
    public string Default;
    private TMP_Text tmp;

    public LocalizedText Init(string key, string defaultValue) {
        Key = key;
        Default = defaultValue;
        UpdateText();
        return this;
    }

    void Awake() => tmp = GetComponent<TMP_Text>();

    void OnEnable() {
        Core.Lang.OnInitialize += UpdateText;
        UpdateText();
    }

    void OnDisable() => Core.Lang.OnInitialize -= UpdateText;

    public void UpdateText() {
        if(tmp != null && !string.IsNullOrEmpty(Key)) {
            tmp.text = Core.Lang.Get(Key, Default);
        }
    }
}