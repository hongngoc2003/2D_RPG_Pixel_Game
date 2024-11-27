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
    }

    protected override void Die() {
        base.Die();

        PlayerManager.instance.player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop(); 
    }

    protected override void DecreaseHealthBy(int _dmg) {
        base.DecreaseHealthBy(_dmg);

        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null) {
            currentArmor.ExecuteItemEffects(PlayerManager.instance.player.transform);
        }
    }

    public override void OnEvasion() {
        PlayerManager.instance.player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamge(CharacterStats _targetStat, float _multiplier) {
        if (TargetCanAvoidAttack(_targetStat))
            return;


        int totalDamage = damage.GetValue() + strength.GetValue();
        if(_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CanCrit()) {
            totalDamage += CalculateCritDmg(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage);

        DoMagicalDmg(_targetStat);

    }
}
