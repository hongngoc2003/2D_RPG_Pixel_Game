using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {
    private Enemy enemy;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentModifier = .4f;
    protected override void Start() {
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();

    }

    private void ApplyLevelModifiers() {
        Modify(strength);
        Modify(agility);
        Modify(intelligent);
        Modify(vitality);

        Modify(damage);
        Modify(critRate);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResist);

        Modify(iceDmg);
        Modify(fireDmg);
        Modify(lightningDmg);
    }

    private void Modify(Stat _stat) {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void TakeDamage(int _damage) {
        base.TakeDamage(_damage);
    }

    protected override void Die() {
        base.Die();
        enemy.Die();
    }
}
