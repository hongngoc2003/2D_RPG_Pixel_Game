using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop {
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;

    public override void GenerateDrop() {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequiped = new List<InventoryItem>();
        List<InventoryItem> materialsToLose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList()) {
            if (Random.Range(0, 100) <= chanceToLoseItems) {
                DropItem(item.data);
                itemsToUnequiped.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequiped.Count; i++) {
            inventory.UnequipItem(itemsToUnequiped[i].data as ItemDataEquipment);
        }

        foreach (InventoryItem item in inventory.GetStashList()) {
            if (Random.Range(0, 100) <= chanceToLoseItems) {
                DropItem(item.data);
                materialsToLose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLose.Count; i++) {
            inventory.RemoveItem(materialsToLose[i].data);
        }
    }
}
