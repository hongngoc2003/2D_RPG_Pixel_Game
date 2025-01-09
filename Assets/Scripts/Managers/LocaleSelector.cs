using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    public static LocaleSelector instance;
    private bool active = false;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
            LoadLocale();
        }
    }

    public void ChangeLocale(int _localeID) {
        if (active == true)
            return;
        StartCoroutine(SetLocale(_localeID));
    }

    IEnumerator SetLocale(int _localeID) {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
        PlayerPrefs.SetInt("LocaleID", _localeID);
        PlayerPrefs.Save();
    }

    private void LoadLocale() {
        int savedLocaleId  = PlayerPrefs.GetInt("LocaleID");
        ChangeLocale(savedLocaleId);
    }
}
