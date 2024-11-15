using UnityEngine;
using UnityEngine.UI;

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
    public Stat magicResist;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critRate;
    public Stat critPower;

    [Header("Magic stats")]
    public Stat fireDmg;
    public Stat iceDmg;
    public Stat lightningDmg;

    public bool isIgnited;
    public bool isFreezed;
    public bool isShocked;

    private float ignitedTimer;
    private float freezedTimer;
    private float shockedTimer;


    private float ignitedDmgCooldown = .3f;
    private float ignitedDmgTimer;
    private int igniteDmg;

    public int currentHealth;

    public System.Action onHealthChanged;

    protected virtual void Start() {
        critPower.SetDefaultValue(150);
        currentHealth = GetFullHealthValue();
    }

    protected virtual void Update() {
        ignitedTimer -= Time.deltaTime;
        freezedTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDmgTimer -= Time.deltaTime;

        if(ignitedTimer < 0 ) {
            isIgnited = false;
        }

        if(freezedTimer < 0 ) {
            isFreezed = false;
        }

        if(shockedTimer < 0 ) {
            isShocked = false;
        }

        if( ignitedDmgTimer < 0 && isIgnited) {
            Debug.Log("burnin " + igniteDmg);

            DecreaseHealthBy(igniteDmg);

            if(currentHealth < 0 ) {
                Die();
            }
            ignitedDmgTimer = ignitedDmgCooldown;
        }
    }

    public void SetupIgniteDmg(int _dmg) => igniteDmg = _dmg;
    public virtual void DoDamage(CharacterStats _targetStat) {
        if (TargetCanAvoidAttack(_targetStat))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit()) {
            totalDamage += CalculateCritDmg(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage);
        DoMagicalDmg(_targetStat);
    }

    public virtual void DoMagicalDmg(CharacterStats _targetStats) {
        int _fireDmg = fireDmg.GetValue();
        int _iceDmg = iceDmg.GetValue();
        int _lightningDmg = lightningDmg.GetValue();

        int totalMagicalDmg = _fireDmg + _iceDmg + _lightningDmg + intelligent.GetValue();
        totalMagicalDmg = CheckTargetResistance(_targetStats, totalMagicalDmg);

        _targetStats.TakeDamage(totalMagicalDmg);

        if(Mathf.Max(_fireDmg, _iceDmg, _iceDmg) <= 0) {
            return;
        }

        bool canApplyIgnite = _fireDmg > _iceDmg && _fireDmg > _lightningDmg;
        bool canApplyFreeze = _iceDmg > _fireDmg && _iceDmg > _lightningDmg;
        bool canApplyShock = _lightningDmg > _fireDmg && _lightningDmg > _iceDmg;

        while(!canApplyIgnite &&  !canApplyFreeze && !canApplyShock) {
            if(Random.value < .5f && _fireDmg > 0) {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                Debug.Log("fire");
                return;
            }

            if (Random.value < .5f && _iceDmg > 0) {
                canApplyFreeze = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                Debug.Log("ice");
                return;
            }

            if (Random.value < .5f && _lightningDmg > 0) {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                Debug.Log("lightning");
                return;
            }

        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDmg(Mathf.RoundToInt(_fireDmg * .2f));

        _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDmg) {
        totalMagicalDmg -= _targetStats.magicResist.GetValue() + (_targetStats.intelligent.GetValue() * 3);
        totalMagicalDmg = Mathf.Clamp(totalMagicalDmg, 0, int.MaxValue);
        return totalMagicalDmg;
    }

    public void ApplyAilment(bool _ignite, bool _freeze, bool _shock) {
        if(isIgnited || isFreezed || isShocked) {
            return;
        }

        if(_ignite) {
            isIgnited = _ignite;
            ignitedTimer = 2f;
        }

        if (isFreezed) {
            isFreezed = _freeze;
            freezedTimer = 2f;
        }

        if(isShocked) {
            isShocked = _shock;
            shockedTimer = 2f;
        }

        isIgnited  = _ignite;
        isFreezed = _freeze;
        isShocked = _shock;
    }
    private int CheckTargetArmor(CharacterStats _targetStat, int totalDamage) {
        if (_targetStat.isFreezed)
            totalDamage -= Mathf.RoundToInt(_targetStat.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStat.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStat) {
        int totalEvasion = _targetStat.evasion.GetValue() + _targetStat.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion) {
            return true;
        }
        return false;
    }
    public virtual void TakeDamage(int _damage) {

        DecreaseHealthBy(_damage);

        Debug.Log(_damage);

        if (currentHealth <= 0) {
            Die();
        }

    }
    protected virtual void DecreaseHealthBy(int _dmg) {
        currentHealth -= _dmg;

        if (onHealthChanged != null) {
            onHealthChanged();
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
    public int GetFullHealthValue() {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
