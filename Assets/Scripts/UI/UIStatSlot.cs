using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private void OnValidate() {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null )
            statNameText.text = statName;
    }

    void Start() {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null) {
            statValueText.text = playerStats.GetStats(statType).GetValue().ToString();


            if(statType == StatType.health)
                statValueText.text = playerStats.GetFullHealthValue().ToString();
            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            if(statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            if (statType == StatType.critRate)
                statValueText.text = (playerStats.critRate.GetValue() + playerStats.agility.GetValue()).ToString();
            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            if (statType == StatType.magicResist)
                statValueText.text = (playerStats.magicResist.GetValue() + playerStats.intelligent.GetValue() * 3).ToString() ;




        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ui.statTooltip.HideStatTooltip();
    }
}
