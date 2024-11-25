using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unlocked;
    private Image skillImage;

    [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;


    private void OnValidate() {
        gameObject.name = "SkillTreeSlot UI - " + skillName;
    }

    private void Start() {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot() {
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
        ui.skillTooltip.ShowToolTip(skillDescription, skillName);
        //Vector2 mousePosition = Input.mousePosition;
        //float xOffset = 0;
        //if (mousePosition.x > 600)
        //    xOffset = -150;
        //else
        //    xOffset = 150;
        //ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + 150);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ui.skillTooltip.HideToolTip();
    }
}
