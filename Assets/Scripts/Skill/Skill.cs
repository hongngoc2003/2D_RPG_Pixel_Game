using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    public float coolDownTimer;

    protected Player player;

    protected virtual void Start() {
        player = PlayerManager.instance.player;

        CheckUnlock();
    }

    protected virtual void Update() {
        coolDownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock() {

    }

    public virtual bool CanUseSkill() {
        if(coolDownTimer < 0) {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }
        player.fx.CreatePopupText("Cooldowning");
        return false;
    }
    public virtual void UseSkill() {
        //
    }

    public virtual Transform FindClosestEnemy(Transform _checkTransform) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance) {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
