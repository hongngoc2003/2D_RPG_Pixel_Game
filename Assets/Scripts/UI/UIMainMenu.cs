using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] UIFadeScreen fadeScreen;
    [SerializeField] private GameObject chooseLevel;
    [SerializeField] private GameObject settingsUI;
    private SettingsData currentSetting;
    private GameData data;
    private void Start() {
        if(SaveManager.instance.HasNoSaveData())
            continueButton.SetActive(false);
        data = SaveManager.instance.LoadGameData();
    }
    public void ContinueGame() {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f, data.lastScene) );
    }

    public void NewGame() {
        chooseLevel.SetActive(true);
        SaveManager.instance.SaveSettings();
        currentSetting = data.settingsData;
        SaveManager.instance.NewGame();
        data.settingsData = currentSetting;
        //StartCoroutine(LoadSceneWithFadeEffect(1.5f, sceneName));
    }

    public void ChooseCastle() => SceneManager.LoadScene("Castle");
    public void ChooseForest() => SceneManager.LoadScene("Forest");

    public void ExitButton() => settingsUI.SetActive(false);
    public void ExitGame() {

    }

    public void Settings() => settingsUI.SetActive(true);
    IEnumerator LoadSceneWithFadeEffect(float _delay,string _sceneName) {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_sceneName);
    }
}
