using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionControl : MonoBehaviour {
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start() {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRate;

        // Lọc độ phân giải theo tần số làm mới
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].refreshRate == currentRefreshRate) {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // Tạo danh sách tùy chọn cho Dropdown
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++) {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width &&
                filteredResolutions[i].height == Screen.height &&
                Mathf.Approximately(filteredResolutions[i].refreshRate, currentRefreshRate)) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Kiểm tra nếu có độ phân giải được lưu trước đó
        if (PlayerPrefs.HasKey("resolutionIndex")) {
            currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    public void SetResolution(int resolutionIndex) {
        currentResolutionIndex = resolutionIndex;
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);

        // Lưu độ phân giải vào PlayerPrefs
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }
}
