using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave {
        public Transform[] enemies;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI wavePointText;

    private int wavePoint = 0;
    public float enemyCountMultiplier = 1.2f;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;

    private void Start() {
        waveCountdown = timeBetweenWaves;
        UpdateWavePointText();
    }

    private void Update() {
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {
                WaveCompleted();
            } else {
                return;
            }
        }

        if (waveCountdown <= 0) {
            if (state != SpawnState.SPAWNING) {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
        UpdateWavePointText();
    }

    private void WaveCompleted() {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        wavePoint++;
        UpdateWavePointText();

        if (nextWave + 1 > waves.Length - 1) {
            for (int i = 0; i < waves.Length; i++) {
                waves[i].count = Mathf.RoundToInt(waves[i].count * enemyCountMultiplier);
            }
            nextWave = 0;
        } else {
            nextWave++;
        }
    }

    private void UpdateWavePointText() {
        if (wavePointText != null) {
            wavePointText.text = "Wave: " + wavePoint;
        }
    }

    private bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave) {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++) {
            SpawnEnemy(_wave.enemies[Random.Range(0, _wave.enemies.Length)]);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    private void SpawnEnemy(Transform _enemy) {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
