using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour {
    private EntityFX fx;

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
    [SerializeField] private float ailmentDuration = 4;

    private float ignitedDmgCooldown = .3f;
    private float ignitedDmgTimer;
    private int igniteDmg;

    private int shockDmg;
    [SerializeField] private GameObject thunderStrikePrefab;
 
    public int currentHealth;

    public System.Action onHealthChanged;

    protected bool isDead;

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
        if (TargetCanAvoidAttack(_targetStat))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit()) {
            totalDamage += CalculateCritDmg(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStat, totalDamage);
        //DoMagicalDmg(_targetStat);
        _targetStat.TakeDamage(totalDamage);
    }
    public virtual void DoMagicalDmg(CharacterStats _targetStats) {
        int _fireDmg = fireDmg.GetValue();
        int _iceDmg = iceDmg.GetValue();
        int _lightningDmg = lightningDmg.GetValue();

        int totalMagicalDmg = _fireDmg + _iceDmg + _lightningDmg + intelligent.GetValue();
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

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");


        if (currentHealth <= 0 && !isDead) {
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
        isDead = true;
    }

    #region Calculation
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDmg) {
        totalMagicalDmg -= _targetStats.magicResist.GetValue() + (_targetStats.intelligent.GetValue() * 3);
        totalMagicalDmg = Mathf.Clamp(totalMagicalDmg, 0, int.MaxValue);
        return totalMagicalDmg;
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
    private int CheckTargetArmor(CharacterStats _targetStat, int totalDamage) {
        if (_targetStat.isFreezed)
            totalDamage -= Mathf.RoundToInt(_targetStat.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStat.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
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
    #endregion
}
