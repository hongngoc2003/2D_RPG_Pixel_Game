using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager {
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEquipmentSlot[] equipmentSlot;
    private UIStatSlot[] statSlot;

    [Header("Item cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipments;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems() {

        foreach (ItemDataEquipment item in loadedEquipments) {
            EquipItem(item);
        }


        if (loadedItems.Count > 0) {
            foreach (InventoryItem item in loadedItems) {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }

        for (int i = 0; i < startingItems.Count; i++) {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item) {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary) {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }
        if (oldEquipment != null) {
            UnequipedItem(oldEquipment);
            AddItem(oldEquipment);
        }


        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateUISlot();
    }
    public void UnequipedItem(ItemDataEquipment itemToRemove) {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value)) {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }
    private void UpdateUISlot() {
        for (int i = 0; i < equipmentSlot.Length; i++) {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary) {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++) {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++) {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++) {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++) {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI() {
        for (int i = 0; i < statSlot.Length; i++) {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item) {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateUISlot();
    }
    private void AddToStash(ItemData _item) {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value)) {
            value.AddStack();
        } else {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }
    private void AddToInventory(ItemData _item) {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) {
            value.AddStack();
        } else {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem(ItemData _item) {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) {
            if (value.stackSize <= 1) {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            } else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue)) {
            if (stashValue.stackSize <= 1) {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            } else
                stashValue.RemoveStack();
        }

        UpdateUISlot();
    }

    public bool CanAddItem() {
        if (inventory.Count >= inventoryItemSlot.Length) {
            return false;
        }

        return true;
    }
    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requiredMaterials) {
        //Ktra xem trong stash co du material can ko
        foreach (var requiredItem in _requiredMaterials)
        {
            if(stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem)) {
                if(stashItem.stackSize < requiredItem.stackSize) {
                    Debug.Log("Not enough materials: " + requiredItem.data.name);
                    return false;
                } 
            } else {
                Debug.Log("Material not found in stash: " + requiredItem.data.name);
                return false;
            }
        }

        //Neu du material thi craft va tru stackSize
        foreach (var requiredMaterial in _requiredMaterials)
        {
            for (int i = 0; i < requiredMaterial.stackSize; i++)
            {
                RemoveItem(requiredMaterial.data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("Craft successfully: " + _itemToCraft.name);
        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
        
    public ItemDataEquipment GetEquipment(EquipmentType _type) {
        ItemDataEquipment equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary) {
            if (item.Key.equipmentType == _type)
                equippedItem = item.Key;
        }

        return equippedItem;
    }

    public void UseFlask() {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask) {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ExecuteItemEffects(null);
            lastTimeUsedFlask = Time.time;
        } else {
            Debug.Log("Flask on cooldown");
        }

    }

    public bool CanUseArmor() {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown) {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on cooldown");
        return false;
    }

    public void LoadData(GameData _data) {
        foreach (KeyValuePair<string, int> pair in _data.inventory) {
            foreach (var item in GetItemDatabase()) {
                if (item != null && item.itemId == pair.Key) {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId) {
            foreach (var item in GetItemDatabase()) {
                if (item != null && loadedItemId == item.itemId) {
                    loadedEquipments.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data) {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary) {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary) {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDictionary) {
            _data.equipmentId.Add(pair.Key.itemId);
        }

    }

    private List<ItemData> GetItemDatabase() {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });
        foreach (string SOName in assetNames) {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }
        return itemDatabase;
    }
}
