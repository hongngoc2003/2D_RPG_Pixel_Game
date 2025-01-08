using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour {
    public Slider bgmSlider; // Thanh trượt cho nhạc nền
    public Slider sfxSlider; // Thanh trượt cho hiệu ứng âm thanh
    public string parameter;

    [SerializeField] private AudioMixer audioMixer; // Mixer âm thanh
    [SerializeField] private float multiplier = 20f; // Hệ số điều chỉnh âm lượng

    private void Start() {
        // Khởi tạo giá trị thanh trượt từ PlayerPrefs
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Thêm listener cho sự thay đổi giá trị thanh trượt
        bgmSlider.onValueChanged.AddListener(value => AudioManager.instance.SetBGMVolume(value));
        sfxSlider.onValueChanged.AddListener(value => AudioManager.instance.SetSFXVolume(value));
    }

    // Cập nhật tham số âm thanh của mixer dựa trên giá trị thanh trượt
    public void SliderValue(float _value) {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }

    // Tải giá trị thanh trượt đã lưu
    public void LoadSlider(float _value) {
        if (_value >= 0.001f)
            bgmSlider.value = _value;
        if (_value >= 0.001f)
            sfxSlider.value = _value;

    }
}