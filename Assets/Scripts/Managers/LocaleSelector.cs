using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour, ISaveManager
{
    public static LocaleSelector instance;

    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private bool active = false;
    private int localeId = 0;
    public void ChangeLocale(int _localeID) {
        if (active == true)
            return;
        StartCoroutine(SetLocale(_localeID));
        localeId = _localeID;                               
    }

    public void LoadData(GameData _data) {
        if (_data != null && _data.localeId >= 0) {
            ChangeLocale(_data.localeId);
        }
    }

    public void SaveData(ref GameData _data) {
        _data.localeId = localeId;
    }

    IEnumerator SetLocale(int _localeID) {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
    }
}
