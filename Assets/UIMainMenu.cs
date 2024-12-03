using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private string sceneName = "MainScene";

    private void Start() {
        if(SaveManager.instance.HasNoSaveData())
            continueButton.SetActive(false);
    }
    public void ContinueGame() {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame() {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {

    }
}
