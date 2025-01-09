using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownLanguage : MonoBehaviour {
    [SerializeField] private TMP_Dropdown dropdown;
    private void Start() {
        // Load giá trị LocaleID từ LocaleSelector (lấy từ PlayerPrefs)
        int savedLocaleID = PlayerPrefs.GetInt("LocaleID"); // Mặc định là 0
        dropdown.value = savedLocaleID; // Cập nhật giao diện Dropdown

        // Gán hàm xử lý khi người dùng thay đổi giá trị
        dropdown.onValueChanged.AddListener((value) => {
            LocaleSelector.instance.ChangeLocale(value); // Gọi LocaleSelector để thay đổi ngôn ngữ
        });
    }
}
