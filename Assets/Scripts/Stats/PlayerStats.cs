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
}
