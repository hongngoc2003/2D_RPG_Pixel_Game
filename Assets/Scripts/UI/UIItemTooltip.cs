using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemTooltip : UITooltip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowTooltip(ItemDataEquipment item) {
        if(item == null)
            return;

        itemNameText.text = item.itemName.GetLocalizedString();
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }
}
