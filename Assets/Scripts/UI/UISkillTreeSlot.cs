using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    
    private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    [SerializeField] private LocalizedString localizedSkillDescription;

    public bool unlocked;
    private Image skillImage;

    [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;


    private void OnValidate() {
        gameObject.name = "SkillTreeSlot UI - " + skillName;
    }

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start() {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if(unlocked) {
            skillImage.color = Color.white;
        }

        localizedSkillDescription.StringChanged += UpdateLocalizedDescription;
        UpdateLocalizedDescription(localizedSkillDescription.GetLocalizedString());
    }

    private void OnDestroy() {
        // Hủy đăng ký sự kiện khi object bị phá hủy
        localizedSkillDescription.StringChanged -= UpdateLocalizedDescription;
    }

    private void UpdateLocalizedDescription(string newDescription) {
        skillDescription = newDescription;
    }

    public void UnlockSkillSlot() {
        if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false)
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false) {
                Debug.Log("cant unlock");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true) {
                Debug.Log("cant unlock");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ui.skillTooltip.ShowToolTip(skillDescription, skillName, skillPrice);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ui.skillTooltip.HideToolTip();
    }

    public void LoadData(GameData _data) {
        if(_data.playerData.skillTree.TryGetValue(skillName, out bool value)) {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data) {
        if(_data.playerData.skillTree.TryGetValue(skillName, out bool value)) {
            _data.playerData.skillTree.Remove(skillName);
            _data.playerData.skillTree.Add(skillName, unlocked);
        }
        else
            _data.playerData.skillTree.Add(skillName, unlocked);
    }
}
