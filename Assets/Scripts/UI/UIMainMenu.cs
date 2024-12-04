using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] UIFadeScreen fadeScreen;
    private void Start() {
        if(SaveManager.instance.HasNoSaveData())
            continueButton.SetActive(false);
    }
    public void ContinueGame() {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame() {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame() {

    }

    IEnumerator LoadSceneWithFadeEffect(float _delay) {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
