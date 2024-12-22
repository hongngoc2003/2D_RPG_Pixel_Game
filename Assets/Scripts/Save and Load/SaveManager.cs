using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour {

    public static SaveManager instance;

    [SerializeField] private string fileName;

    private GameData gameData;

    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSaveData() {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start() {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManager();

        LoadGame();
    }
    public void NewGame() {
        gameData = new GameData();
    }

    public void LoadGame() {

        gameData = dataHandler.Load();

        if (this.gameData == null) {
            Debug.Log("No saved data found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers) {
            saveManager.LoadData(gameData);
        }
    }

    public GameData LoadGameData() {
        gameData = dataHandler.Load(); // Tải dữ liệu từ file
        if (this.gameData == null) {
            Debug.Log("No saved data found");
            NewGame(); // Nếu không có dữ liệu cũ, tạo dữ liệu mới
        }
        return gameData;  // Trả về gameData đã tải
    }

    public void SaveGame() {
        foreach (ISaveManager saveManager in saveManagers) {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void SaveSettings() {
        LocaleSelector.instance.SaveData(ref gameData);
        AudioManager.instance.SaveData(ref gameData);
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManager() {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasNoSaveData() {
        if (dataHandler.Load() == null) {
            return true;
        }

        return false;
    }
}
