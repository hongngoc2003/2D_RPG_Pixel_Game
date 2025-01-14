using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Localization.Settings;

[System.Serializable]
public class GameData {
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public string lastScene;

    public GameData() {
        this.currency = 0;

        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;
        lastScene = string.Empty;
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        lostCurrencyAmount = 0;
    }
}

