using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Localization.Settings;

[System.Serializable]
public class PlayerData {
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public PlayerData() {
        this.currency = 0;

        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;

        lostCurrencyX = 0;
        lostCurrencyY = 0;
        lostCurrencyAmount = 0;
    }
}

[System.Serializable]
public class SettingsData {
    public SerializableDictionary<string, float> volumeSettings;
    public int localeId;

    public SettingsData() {
        localeId = 0; // Mặc định là ngôn ngữ đầu tiên trong Localization
        volumeSettings = new SerializableDictionary<string, float>();
    }
}

[System.Serializable]
public class GameData {
    public string lastScene;
    public PlayerData playerData;
    public SettingsData settingsData;

    public GameData() {
        lastScene = "MainMenu";
        playerData = new PlayerData();
        settingsData = new SettingsData();
    }
}
