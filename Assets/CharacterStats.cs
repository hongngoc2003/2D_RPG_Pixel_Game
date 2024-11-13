using UnityEngine;

public class CharacterStats : MonoBehaviour {
    [Header("Major stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligent;
    public Stat vitality;

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critRate;
    public Stat critPower;


    [SerializeField] private int currentHealth;

    protected virtual void Start() {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStat) {
        if (TargetCanAvoidAttack(_targetStat))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit()) {
            totalDamage += CalculateCritDmg(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage);
    }

    private static int CheckTargetArmor(CharacterStats _targetStat, int totalDamage) {
        totalDamage -= _targetStat.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStat) {
        int totalEvasion = _targetStat.evasion.GetValue() + _targetStat.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion) {
            return true;
        }
        return false;
    }
    public virtual void TakeDamage(int _damage) {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth <= 0) {
            Die();
        }
    }
    protected virtual void Die() {

    }
    private bool CanCrit() {
        int totalCritChance = critRate.GetValue() + agility.GetValue();

        if(Random.Range(0,100) <= totalCritChance) {
            return true;
        }

        return false;
    }
    private int CalculateCritDmg(int _damage) {
        float totalCritPower = (this.critPower.GetValue() + strength.GetValue()) * .01f;
        float critDmg = _damage * totalCritPower;

        return Mathf.RoundToInt(critDmg);
    }
}
