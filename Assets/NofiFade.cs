using UnityEngine;
using TMPro;
using System.Collections;

public class NofiFade : MonoBehaviour {
    private TextMeshProUGUI text; // Nếu dùng TextMeshPro cho UI
    private TextMeshPro text3D; // Nếu dùng TextMeshPro 3D (World Space)

    [SerializeField] private float fadeDuration = 1f; // Thời gian fade
    [SerializeField] private float displayTime = 2f; // Thời gian hiển thị trước khi fade out
    [SerializeField] private bool autoFadeOut = true; // Tự động fade out sau khi hiển thị

    private void Awake() {
        // Kiểm tra đối tượng TextMeshPro
        text = GetComponent<TextMeshProUGUI>();
        text3D = GetComponent<TextMeshPro>();

        if (text == null && text3D == null) {
            Debug.LogError("Không tìm thấy TextMeshPro trên GameObject!");
            enabled = false;
        }
    }

    private void OnEnable() {
        // Đặt alpha về 0 và bắt đầu fade in
        SetAlpha(0);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        float timer = 0f;
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0, 1, timer / fadeDuration)); // Tăng dần alpha từ 0 -> 1
            yield return null;
        }

        if (autoFadeOut) {
            // Đợi trước khi fade out
            yield return new WaitForSeconds(displayTime);
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut() {
        float timer = 0f;
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1, 0, timer / fadeDuration)); // Giảm dần alpha từ 1 -> 0
            yield return null;
        }

        gameObject.SetActive(false); // Tùy chọn: Tắt text sau khi fade out
    }

    private void SetAlpha(float alpha) {
        if (text != null) {
            // Nếu là TextMeshPro UI
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        } else if (text3D != null) {
            // Nếu là TextMeshPro 3D
            text3D.color = new Color(text3D.color.r, text3D.color.g, text3D.color.b, alpha);
        }
    }

    // Gọi hàm này để khởi động lại hiệu ứng fade
    public void StartFade() {
        StopAllCoroutines();
        SetAlpha(0);
        StartCoroutine(FadeIn());
    }
}
