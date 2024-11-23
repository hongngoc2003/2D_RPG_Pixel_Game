 using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemText;

    public InventoryItem item;

    private UI ui;

    private void Start() {
        ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem) {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null) {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1) {
                itemText.text = item.stackSize.ToString();
            } else {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot() {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }


    public virtual void OnPointerDown(PointerEventData eventData) {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl)) {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        if(item.data.itemType == ItemType.Equipment) {
            Inventory.instance.EquipItem(item.data);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(item == null) return;
        ui.itemTooltip.ShowTooltip(item.data as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(item == null) return;
        ui.itemTooltip.HideTooltip();
    }
}
