using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderExplodeController : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start() {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Enemy>() != null) {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDmg(enemyTarget);
        }
    }
}
