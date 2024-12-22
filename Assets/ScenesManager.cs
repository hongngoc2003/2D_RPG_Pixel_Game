using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour, ISaveManager
{
    public static ScenesManager instance;

    public void SaveData(ref GameData _data) {
        string currentScene = SceneManager.GetActiveScene().name; // Lấy tên scene hiện tại
        _data.lastScene = currentScene; // Lưu vào GameData
    }

    public void LoadData(GameData _data) {
        //if (!string.IsNullOrEmpty(_data.lastScene)) {
        //    SceneManager.LoadScene(_data.lastScene);
        //} else {
        //    Debug.LogWarning("Không có scene trước đó được lưu!");
        //}
    }


    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


}
