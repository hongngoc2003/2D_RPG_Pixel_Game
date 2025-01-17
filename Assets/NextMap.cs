using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMap : MonoBehaviour {
    private float searchCountdown = 1f;
    private bool enemiesAlive = true;

    private void OnTriggerEnter2D(Collider2D collision) {
        // Kiểm tra nếu người chơi va chạm và không còn kẻ thù
        if (collision.GetComponent<Player>() != null && !EnemyIsAlive()) {
            TransferToNextMap();
        }
    }

    private void Update() {
        // Kiểm tra trạng thái kẻ thù mỗi giây
        EnemyIsAlive();
    }

    private void TransferToNextMap() {
        // Lưu game và chuyển cảnh
        SaveManager.instance.SaveGame();
        ScenesManager.instance.LoadScene("Forest");
    }

    private bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;

        // Mỗi giây kiểm tra lại số lượng kẻ thù
        if (searchCountdown <= 0) {
            searchCountdown = 1f;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Kiểm tra nếu không còn kẻ thù nào
            if (enemies.Length == 0 && enemiesAlive) {
                enemiesAlive = false;
                Debug.Log("No enemies left!");
            } else if (enemies.Length > 0 && !enemiesAlive) {
                enemiesAlive = true;
                Debug.Log("Enemies still alive: " + enemies.Length);
            }
        }
        return enemiesAlive;
    }
}
