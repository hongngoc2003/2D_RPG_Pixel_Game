using System.Collections;
using System.Collections.Generic;
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

    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligent;
    public int vitality;

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


    public void AddModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligent.AddModifier(intelligent);
        playerStats.vitality.AddModifier(vitality);

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

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligent.RemoveModifier(intelligent);
        playerStats.vitality.RemoveModifier(vitality);

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

    public void ExecuteItemEffects() {

        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }
}