using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] UIFadeScreen fadeScreen;
    [SerializeField] private GameObject chooseLevel;
    private GameData data;
    private void Start() {
        if(SaveManager.instance.HasNoSaveData())
            continueButton.SetActive(false);
        data = SaveManager.instance.LoadGameData();
    }
    public void ContinueGame() {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f, data.lastScene));
    }

    public void NewGame() {    
        SaveManager.instance.DeleteSaveData();
        chooseLevel.SetActive(true);
        //StartCoroutine(LoadSceneWithFadeEffect(1.5f, sceneName));
    }

    public void ChooseCastle() => SceneManager.LoadScene("Castle");
    public void ChooseForest() => SceneManager.LoadScene("Forest");

    public void ExitGame() {

    }

    public void Settings() {
        
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay,string _sceneName) {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_sceneName);
    }
}
