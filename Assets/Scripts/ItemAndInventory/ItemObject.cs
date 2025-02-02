using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisual() {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName.GetLocalizedString();
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity) {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisual();
    }

    public void PickUpItem() {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment) {
            rb.velocity = new Vector2(0, 7);
            PlayerManager.instance.player.fx.CreatePopupText("Inventory is full");
            return;
        }

        AudioManager.instance.PlaySFX(18,transform);

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
