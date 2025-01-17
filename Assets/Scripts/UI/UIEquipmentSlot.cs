using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot {
    public EquipmentType slotType;

    private void OnValidate() {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData) {
        if (item == null || item.data == null)
            return;

        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        ui.itemTooltip.HideTooltip();
        CleanUpSlot();


    }
}
