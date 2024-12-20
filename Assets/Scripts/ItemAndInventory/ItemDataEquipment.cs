using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType {
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData {
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResist;

    [Header("Offensive stats")]
    public int damage;
    public int critRate;
    public int critPower;

    [Header("Magic stats")]
    public int fireDmg;
    public int iceDmg;
    public int lightningDmg;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;

    public void AddModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.damage.AddModifier(damage);
        playerStats.critRate.AddModifier(critRate);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResist.AddModifier(magicResist);

        playerStats.lightningDmg.AddModifier(lightningDmg);
        playerStats.iceDmg.AddModifier(iceDmg);
        playerStats.fireDmg.AddModifier(fireDmg);

    }

    public void RemoveModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.damage.RemoveModifier(damage);
        playerStats.critRate.RemoveModifier(critRate);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResist.RemoveModifier(magicResist);

        playerStats.lightningDmg.RemoveModifier(lightningDmg);
        playerStats.iceDmg.RemoveModifier(iceDmg);
        playerStats.fireDmg.RemoveModifier(fireDmg);

    }

    public void ExecuteItemEffects(Transform _enemyPosition) {

        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public override string GetDescription() {
        sb.Length = 0;
        descriptionLength = 0;
      
        AddDescription(health, "Health");
        AddDescription(armor, "Armor");
        AddDescription(evasion, "Evasion");
        AddDescription(magicResist, "Magic Res");

        
        AddDescription(damage, "Damage");
        AddDescription(critRate, "Crit Rate");
        AddDescription(critPower, "Crit Power");

        
        AddDescription(fireDmg, "Fire Dmg");
        AddDescription(iceDmg, "Ice Dmg");
        AddDescription(lightningDmg, "Lightning Dmg");

        sb.AppendLine();



        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0) {
                sb.AppendLine();
                sb.AppendLine(itemEffects[i].effectDescription);
                descriptionLength++;
            }

        }


        if (descriptionLength < 5) {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddDescription(int _value, string _name) {
        if(_value != 0) {
            if(sb.Length > 0) 
                sb.AppendLine();

            if(_value > 0 )
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }
}