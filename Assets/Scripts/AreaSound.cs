using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D collision) {
        AudioManager.instance.PlaySFX(areaSoundIndex, null);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        AudioManager.instance.StopSFXWithTime(areaSoundIndex);
    }
}
