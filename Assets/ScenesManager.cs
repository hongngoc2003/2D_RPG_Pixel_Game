using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour, ISaveManager
{
    public static ScenesManager instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    private float target;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); 
        } else {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
    }

    private void Update() {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

    public async void LoadScene(string sceneName) {
        target = 0;
        progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        do {
            await Task.Delay(100);
            target = scene.progress;

        } while (scene.progress < .9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        loaderCanvas.SetActive(false);
    }

    public void SaveData(ref GameData _data) {
        string currentScene = SceneManager.GetActiveScene().name;

        // Kiểm tra nếu scene không phải MainMenu
        if (currentScene != "MainMenu" && currentScene != "EndlessMode") {
            _data.lastScene = currentScene;
        } else {
            Debug.Log("Không lưu scene MainMenu && Endless");
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
