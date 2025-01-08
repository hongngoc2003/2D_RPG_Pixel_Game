using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    public static LocaleSelector instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
            LoadLocale();
        }
    }

    private bool active = false;
    private int localeId = 0;
    public void ChangeLocale(int _localeID) {
        if (active == true)
            return;
        StartCoroutine(SetLocale(_localeID));
        localeId = _localeID;                               
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
        PlayerPrefs.GetInt("LocaleID", 1);
    }
}
