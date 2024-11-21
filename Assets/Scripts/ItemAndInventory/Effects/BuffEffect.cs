using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType{
    strength,
    agility, 
    intelligent, 
    vitality,
    maxHealth,
    armor, 
    evasion,
    magicResist, 
    damage, 
    critRate, 
    critPower, 
    fireDmg, 
    iceDmg, 
    lightningDmg
}

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]


public class BuffEffect : ItemEffect
{

    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition) {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());

    }

    private Stat StatToModify() {
        if (buffType == StatType.strength) return stats.strength;
        else if (buffType == StatType.agility) return stats.agility;
        else if (buffType == StatType.intelligent) return stats.intelligent;
        else if (buffType == StatType.vitality) return stats.vitality;
        else if (buffType == StatType.maxHealth) return stats.maxHealth;
        else if (buffType == StatType.armor) return stats.armor;
        else if ((buffType == StatType.damage)) return stats.damage;
        else if ((buffType == StatType.critRate)) return stats.critRate;
        else if (buffType == StatType.critPower) return stats.critPower;
        else if (buffType == StatType.iceDmg) return stats.iceDmg;
        else if (buffType == StatType.fireDmg) return stats.fireDmg;
        else if (buffType == StatType.lightningDmg) return stats.lightningDmg;
        else if (buffType == StatType.magicResist) return stats.magicResist;
        else if (buffType == StatType.evasion) return stats.evasion;
        else if (buffType == StatType.magicResist) return stats.magicResist;

        return null;
    }
}
