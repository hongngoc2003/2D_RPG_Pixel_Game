using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour {

    [Header("End screen")]
    [SerializeField] private UIFadeScreen fadeScreen;
    [SerializeField] private GameObject endText;

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
        SwitchTo(skillTreeUI); //can assign event ben uiskilltreeslot truoc khi assign event o skill script
    }

    private void Start() {
        SwitchTo(ingameUI);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) 
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);

    }
    public void SwitchTo(GameObject _menu) {



        for (int i = 0; i < transform.childCount; i++) {
            bool fadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);
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
            if (transform.GetChild(i).gameObject.activeSelf)
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
    }
}
