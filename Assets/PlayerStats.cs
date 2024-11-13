using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    protected override void Start() {
        base.Start();
    }
    public override void TakeDamage(int _damage) {
        base.TakeDamage(_damage);

        PlayerManager.instance.player.DamageEffect();
    }

    protected override void Die() {
        base.Die();

        PlayerManager.instance.player.Die();
    }
}
