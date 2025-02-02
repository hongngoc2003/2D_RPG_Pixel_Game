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

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop(); 
    }

    protected override void DecreaseHealthBy(int _dmg) {
        base.DecreaseHealthBy(_dmg);

        if(_dmg > GetFullHealthValue() * .3f) {
            PlayerManager.instance.player.SetupKnockbackPower(new Vector2(5,5));
            PlayerManager.instance.player.fx.ScreenShake(PlayerManager.instance.player.fx.shakeHighDmg);

        }

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


        int totalDamage = damage.GetValue();
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
