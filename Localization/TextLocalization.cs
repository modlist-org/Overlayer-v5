using UnityEngine;
using TMPro;

namespace Overlayer.Localization;

public class TextLocalization : MonoBehaviour {
    public string Key;
    public string Default;
    private TMP_Text tmp;

    public TextLocalization Init(string key, string defaultValue) {
        Key = key;
        Default = defaultValue;
        UpdateText();
        return this;
    }

    void Awake() => tmp = GetComponent<TMP_Text>();

    void OnEnable() {
        Core.Tr.OnInitialize += UpdateText;
        UpdateText();
    }

    void OnDisable() => Core.Tr.OnInitialize -= UpdateText;

    public void UpdateText() {
        if(tmp != null && !string.IsNullOrEmpty(Key)) {
            tmp.text = Core.Tr.Get(Key, Default);
        }
    }
}