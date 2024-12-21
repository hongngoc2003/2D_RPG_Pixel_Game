using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Localization.Settings;

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

    public SerializableDictionary<string, float> volumeSettings;

    public int localeId;

    public GameData() {
        localeId = 0;

        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
