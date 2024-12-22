using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    public string closestCheckpointId;

    [SerializeField] private Checkpoint[] checkpoints;

    [Header("Last fall")]
    [SerializeField] private GameObject lastFallPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Start() {
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.N))
            RestartScene();
    }
    public void RestartScene() {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckpoint(GameData _data) {
        foreach (KeyValuePair<string, bool> pair in _data.playerData.checkpoints) {
            foreach (Checkpoint checkpoint in checkpoints) {
                if (checkpoint.checkpointId == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData _data) {
        lostCurrencyAmount = _data.playerData.lostCurrencyAmount;
        lostCurrencyX = _data.playerData.lostCurrencyX;
        lostCurrencyY = _data.playerData.lostCurrencyY;

        if(lostCurrencyAmount > 0) {
            GameObject newLastFall = Instantiate(lastFallPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLastFall.GetComponent<LastFallController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data) {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoint(_data);
        PlacePlayerAtClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    private void PlacePlayerAtClosestCheckpoint(GameData _data) {
        if (_data.playerData.closestCheckpointId == null)
            return;

        closestCheckpointId = _data.playerData.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints) {
            if (closestCheckpointId == checkpoint.checkpointId)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }

    public void SaveData(ref GameData _data) {
        _data.playerData.lostCurrencyAmount = lostCurrencyAmount;
        _data.playerData.lostCurrencyX = PlayerManager.instance.player.transform.position.x;
        _data.playerData.lostCurrencyY = PlayerManager.instance.player.transform.position.y;

        if(FindClosestCheckpoint() != null)
            _data.playerData.closestCheckpointId = FindClosestCheckpoint().checkpointId;

        _data.playerData.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.playerData.checkpoints.Add(checkpoint.checkpointId, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint() {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
            if(closestDistance > distanceToCheckpoint && checkpoint.activationStatus == true) {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }

    public void PauseGame(bool _pause) {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
