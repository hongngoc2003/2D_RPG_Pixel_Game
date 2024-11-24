using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot {
    protected override void Start() {
        base.Start();

    }

    public void SetupCraftSlot(ItemDataEquipment _data) {
        if (_data == null) {
            return;
        }

        item.data = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }
    private void OnEnable() {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData) {

        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment );
    }
}
