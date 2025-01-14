using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum Scenes{
    MainMenu,
    Castle,
    Forest
}
public class ScenesManager : MonoBehaviour, ISaveManager
{
    public static ScenesManager instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); 
        } else {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
    }

    public void SaveData(ref GameData _data) {
        string currentScene = SceneManager.GetActiveScene().name;

        // Kiểm tra nếu scene không phải MainMenu
        if (currentScene != Scenes.MainMenu.ToString()) {
            _data.lastScene = currentScene;
        } else {
            Debug.Log("Không lưu scene MainMenu.");
        }
    }

    public void LoadData(GameData _data) {
        //if (!string.IsNullOrEmpty(_data.lastScene)) {
        //    string savedScene = _data.lastScene;

        //    // Kiểm tra nếu scene hợp lệ (không phải MainMenu)
        //    if (savedScene != Scenes.MainMenu.ToString()) {
        //        SceneManager.LoadScene(savedScene);
        //    } else {
        //        Debug.LogWarning("Không có scene hợp lệ được lưu");
        //    }
        //} else {
        //    Debug.LogWarning("Không có scene trước đó được lưu!");
        //}
    }



}
