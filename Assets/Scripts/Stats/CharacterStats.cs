using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
public enum StatType {
    health,
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

public class CharacterStats : MonoBehaviour {
    private EntityFX fx;

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
    [SerializeField] private float ailmentDuration = 4;

    private float ignitedDmgCooldown = .3f;
    private float ignitedDmgTimer;
    private int igniteDmg;

    private int shockDmg;
    [SerializeField] private GameObject thunderStrikePrefab;
 
    public int currentHealth;

    public System.Action onHealthChanged;

    public bool isDead {  get; private set; }

    private bool isVulnerable;

    public bool isInvicible {  get; private set; }
    protected virtual void Start() {
        critPower.SetDefaultValue(150);
        currentHealth = GetFullHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update() {
        ignitedTimer -= Time.deltaTime;
        freezedTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDmgTimer -= Time.deltaTime;

        if (ignitedTimer < 0) {
            isIgnited = false;
        }

        if (freezedTimer < 0) {
            isFreezed = false;
        }

        if (shockedTimer < 0) {
            isShocked = false;
        }

        if(isIgnited)
            ApplyIgniteDmg();
    }


    public void SetupIgniteDmg(int _dmg) => igniteDmg = _dmg;
    public void SetupThunderStrikeDmg(int _dmg) => shockDmg = _dmg;
    public virtual void DoDamage(CharacterStats _targetStat) {

        if(_targetStat.isInvicible)
            return;

        if (TargetCanAvoidAttack(_targetStat))
            return;

        _targetStat.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue();

        if(CanCrit()) {
            totalDamage += CalculateCritDmg(totalDamage);
            fx.CreateCriticalHitFX(_targetStat.transform);
        }

        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        _targetStat.TakeDamage(totalDamage);

        DoMagicalDmg(_targetStat);
    }
    public virtual void DoMagicalDmg(CharacterStats _targetStats) {
        int _fireDmg = fireDmg.GetValue();
        int _iceDmg = iceDmg.GetValue();
        int _lightningDmg = lightningDmg.GetValue();

        int totalMagicalDmg = _fireDmg + _iceDmg + _lightningDmg;
        totalMagicalDmg = CheckTargetResistance(_targetStats, totalMagicalDmg);

        _targetStats.TakeDamage(totalMagicalDmg);

        if (Mathf.Max(_fireDmg, _iceDmg, _lightningDmg) <= 0) {
            return;
        }

        ApplyAilmentLogic(_targetStats, _fireDmg, _iceDmg, _lightningDmg);
    }


    #region Ailment apply
    private void ApplyIgniteDmg() {
        if (ignitedDmgTimer < 0) {
            DecreaseHealthBy(igniteDmg);

            if (currentHealth < 0 && !isDead) {
                Die();
            }
            ignitedDmgTimer = ignitedDmgCooldown;
        }
    }

    private void ApplyAilmentLogic(CharacterStats _targetStats, int _fireDmg, int _iceDmg, int _lightningDmg) {
        bool canApplyIgnite = _fireDmg > _iceDmg && _fireDmg > _lightningDmg;
        bool canApplyFreeze = _iceDmg > _fireDmg && _iceDmg > _lightningDmg;
        bool canApplyShock = _lightningDmg > _fireDmg && _lightningDmg > _iceDmg;

        while (!canApplyIgnite && !canApplyFreeze && !canApplyShock) {
            if (Random.value < .3f && _fireDmg > 0) {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDmg > 0) {
                canApplyFreeze = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightningDmg > 0) {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
                return;
            }

        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDmg(Mathf.RoundToInt(_fireDmg * .2f));
        if (canApplyShock)
            _targetStats.SetupThunderStrikeDmg(Mathf.RoundToInt(_lightningDmg * .1f));


        _targetStats.ApplyAilment(canApplyIgnite, canApplyFreeze, canApplyShock);
    }

    public void ApplyAilment(bool _ignite, bool _freeze, bool _shock) {
        bool canApplyIgnite = !isIgnited && !isFreezed && !isShocked;
        bool canApplyFreeze = !isIgnited && !isFreezed && !isShocked;
        bool canApplyShock = !isIgnited && !isFreezed;

        if(_ignite && canApplyIgnite) {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration;

            fx.IgniteFXFor(ailmentDuration);
        }

        if (_freeze && canApplyFreeze) {
            isFreezed = _freeze;
            freezedTimer = ailmentDuration;

            float slowPercent = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercent, ailmentDuration);
            fx.FreezeFXFor(ailmentDuration);
        }

        if(_shock && canApplyShock) {
            if(!isShocked) {
                ApplyShock(_shock);
            } else {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithThunderStrike();
            }
        }



        isIgnited  = _ignite;
        isFreezed = _freeze;
        isShocked = _shock;
    }

    public void ApplyShock(bool _shock) {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentDuration;

        fx.ShockFXFor(ailmentDuration);
    }

    private void HitNearestTargetWithThunderStrike() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > .1f) {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance) {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null) {
            GameObject newThunderStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);
            newThunderStrike.GetComponent<ThunderStrikeController>().Setup(shockDmg, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion

    public virtual void TakeDamage(int _damage) {

        if(isInvicible) {
            return;
        }

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");


        if (currentHealth <= 0 && !isDead) {
            Die();
        }

    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCoroutine(_duration));

    private IEnumerator VulnerableForCoroutine(float _duration) {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    protected virtual void DecreaseHealthBy(int _dmg) {
        if(isVulnerable) 
            _dmg = Mathf.RoundToInt(_dmg * 1.1f);

        if (_dmg > 0)
            fx.CreatePopupText(_dmg.ToString());
        
        currentHealth -= _dmg;

        if (onHealthChanged != null) 
            onHealthChanged();
    }

    public virtual void IncreaseHealthBy(int _amount) {
        currentHealth += _amount;

        if(currentHealth > GetFullHealthValue())
            currentHealth = GetFullHealthValue();

        if (onHealthChanged != null) 
            onHealthChanged();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify) {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify) {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
    protected virtual void Die() {
        isDead = true;
    }
    public void KillEntity() {
        if (!isDead)
            Die();  
    }

    public void MakeInvicible(bool _invicible) => isInvicible = _invicible;

    #region Calculation
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDmg) {
        totalMagicalDmg -= _targetStats.magicResist.GetValue();
        totalMagicalDmg = Mathf.Clamp(totalMagicalDmg, 0, int.MaxValue);
        return totalMagicalDmg;
    }
    
    public virtual void OnEvasion() {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStat) {
        int totalEvasion = _targetStat.evasion.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion) {
            _targetStat.OnEvasion();
            return true;
        }
        return false;
    }
    protected int CheckTargetArmor(CharacterStats _targetStat, int totalDamage) {
        if (_targetStat.isFreezed)
            totalDamage -= Mathf.RoundToInt(_targetStat.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStat.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    protected bool CanCrit() {
        int totalCritChance = critRate.GetValue();

        if(Random.Range(0,100) <= totalCritChance) {
            return true;
        }

        return false;
    }
    protected int CalculateCritDmg(int _damage) {
        float totalCritPower = (this.critPower.GetValue()) * .01f;
        float critDmg = _damage * totalCritPower;

        return Mathf.RoundToInt(critDmg);
    }
    public int GetFullHealthValue() {
        return maxHealth.GetValue();
    }
    #endregion

    public Stat GetStats(StatType _statType) {
        if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if ((_statType == StatType.damage)) return damage;
        else if ((_statType == StatType.critRate)) return critRate;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.iceDmg) return iceDmg;
        else if (_statType == StatType.fireDmg) return fireDmg;
        else if (_statType == StatType.lightningDmg) return lightningDmg;
        else if (_statType == StatType.magicResist) return magicResist;
        else if (_statType == StatType.evasion) return evasion;

        return null;
    }

}
