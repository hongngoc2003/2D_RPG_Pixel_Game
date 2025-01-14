using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class UI : MonoBehaviour{

    [Header("End screen")]
    [SerializeField] private UIFadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject ingameUI;

    public UISkillTooltip skillTooltip;
    public UIItemTooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UICraftWindow craftWindow;

    private void Awake() {
        OpenOptionUIAndSkillTreeUI();

        fadeScreen.gameObject.SetActive(true);
    }

    private void Start() {
        SwitchTo(ingameUI);
        CheckForIngameUI();

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }
    private void Update() {
        if (UserInput.instance.characterInput) 
            SwitchWithKeyTo(characterUI);

        if (UserInput.instance.craftInput)
            SwitchWithKeyTo(craftUI);

        if (UserInput.instance.skilltreeInput)
            SwitchWithKeyTo(skillTreeUI);

        if (UserInput.instance.optionsInput)
            SwitchWithKeyTo(optionUI);

    }
    public void SwitchTo(GameObject _menu) {

        for (int i = 0; i < transform.childCount; i++) {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
            if(fadeScreen == false)
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

        if(GameManager.instance != null) {
            if (_menu == ingameUI)
                GameManager.instance.PauseGame(false);
            else 
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu) {
        if (_menu != null && _menu.activeSelf) {
            _menu.SetActive(false);
            CheckForIngameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForIngameUI() {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UIFadeScreen>() == null)
                return;
        }

        SwitchTo(ingameUI);
    }

    public void SwitchOnEndScreen() {
        fadeScreen.FadeOut();
        StartCoroutine(EndSreenCoroutine());
    }

    IEnumerator EndSreenCoroutine() {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void ExitUI(GameObject _ui) {
        _ui.SetActive(false);
        CheckForIngameUI();
        GameManager.instance.PauseGame(false);
    }

    private void OpenOptionUIAndSkillTreeUI() { // the SkillTreeUI was NOT active when getting SaveManagers. so the fix would be to activate it before getting the list of save manager
        optionUI.SetActive(true);
        skillTreeUI.SetActive(true);
    }
}
