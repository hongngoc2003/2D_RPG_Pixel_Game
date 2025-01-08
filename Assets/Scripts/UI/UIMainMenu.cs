using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] UIFadeScreen fadeScreen;
    [SerializeField] private TextMeshProUGUI noSavefileNofi;
    [SerializeField] private GameObject chooseLevel;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject mainMenuUI;
    private GameData data;
    private void Start() {     
        noSavefileNofi.gameObject.SetActive(false);
        data = SaveManager.instance.LoadGameData();
        SwitchTo(settingsUI);
        SwitchTo(mainMenuUI);
    }
    public void SwitchTo(GameObject _menu) {

        for (int i = 0; i < transform.childCount; i++) {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null) {
            if (AudioManager.instance != null) {
                AudioManager.instance.PlaySFX(7, null);
            } else {
                Debug.LogWarning("AudioManager instance is null.");
            }
            _menu.SetActive(true);
        }
    }
    public void ContinueGame() {
        if (SaveManager.instance.HasNoSaveData() || data.lastScene == "MainMenu") {
            noSavefileNofi.gameObject.SetActive(true); // Tự động fade in và fade out
        } else {
            StartCoroutine(LoadSceneWithFadeEffect(1.5f, data.lastScene));
        }
    }
   

    public void NewGame() {
        chooseLevel.SetActive(true);
    }

    public void ChooseCastle() {
        SaveManager.instance.NewGame();
        SceneManager.LoadScene("Castle");  
    }
    public void ChooseForest() {
        SaveManager.instance.NewGame();
        SceneManager.LoadScene("Forest");
    }

    public void CloseButtonSettings() => settingsUI.SetActive(false);
    public void CloseButtonChooseLevel() => chooseLevel.SetActive(false);
    public void ExitGame() {
        Application.Quit();
    }

    public void Settings() => settingsUI.SetActive(true);
    IEnumerator LoadSceneWithFadeEffect(float _delay,string _sceneName) {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_sceneName);
    }
}
