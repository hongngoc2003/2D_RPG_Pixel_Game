 using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            //DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
        }
    }
    private void Start() {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManager();

        LoadGame();
    }
    public void NewGame() {
        DeleteSaveData();
        gameData = new GameData();
    }

    public void LoadGame() {
        if (SceneManager.GetActiveScene().name == "EndlessMode") {
            return;
        }

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
        }
        return gameData;  // Trả về gameData đã tải
    }

    public void SaveGame() {
        if (SceneManager.GetActiveScene().name == "EndlessMode" && SceneManager.GetActiveScene().name == "MainMenu") {
            Debug.Log("Skipping save in endless mode");
            return;
        }

        foreach (ISaveManager saveManager in saveManagers) {
            saveManager.SaveData(ref gameData);
        }

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
