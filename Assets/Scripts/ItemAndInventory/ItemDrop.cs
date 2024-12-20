using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDrop;
    [SerializeField] private ItemData[] possibleDrops;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop() {
        if (possibleDrops.Length == 0)
            return;

        foreach (ItemData item in possibleDrops)
        {
            if(item != null && Random.Range(0,100) < item.dropChance)
                dropList.Add(item);
        }

        for (int i = 0; i < amountOfDrop; i++)
        {
            if(dropList.Count > 0) {
                int randomIndex = Random.Range(0, dropList.Count);
                ItemData itemToDrop = dropList[randomIndex];

                DropItem(itemToDrop);
                dropList.Remove(itemToDrop);
            }
        }
    }
    protected void DropItem(ItemData _itemData) {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
