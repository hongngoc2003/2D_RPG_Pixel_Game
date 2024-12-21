using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class UIStatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private LocalizedString localizedStatDescription;

    private string statDescription;
    private void OnValidate() {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null )
            statNameText.text = statName;
    }

    void Start() {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
        localizedStatDescription.StringChanged += UpdateLocalizedDescription;
        UpdateLocalizedDescription(localizedStatDescription.GetLocalizedString());
    }

    private void OnDestroy() {
        // Hủy đăng ký sự kiện khi object bị phá hủy
        localizedStatDescription.StringChanged -= UpdateLocalizedDescription;
    }

    private void UpdateLocalizedDescription(string newDescription) {
        statDescription = newDescription;
    }

    public void UpdateStatValueUI() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null) {
            statValueText.text = playerStats.GetStats(statType).GetValue().ToString();


            if(statType == StatType.health)
                statValueText.text = playerStats.GetFullHealthValue().ToString();
            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue()).ToString();
            if(statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue()).ToString();
            if (statType == StatType.critRate)
                statValueText.text = (playerStats.critRate.GetValue()).ToString();
            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue()).ToString();
            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue()).ToString();
            if (statType == StatType.magicResist)
                statValueText.text = (playerStats.magicResist.GetValue()).ToString() ;

        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ui.statTooltip.HideStatTooltip();
    }
}
