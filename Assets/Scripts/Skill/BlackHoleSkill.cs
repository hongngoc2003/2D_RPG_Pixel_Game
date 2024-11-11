using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill {
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float growSpeed;
    [Space]
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float blackHoleDuration;

    BlackHoleSkillController currentBlackHole;

    public override bool CanUseSkill() {
        return base.CanUseSkill();
    }

    public override void UseSkill() {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();

        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, attackCooldown, blackHoleDuration);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public bool SkillCompleted() {
        if (!currentBlackHole) 
            return false;
        
        if (currentBlackHole.playerCanExitState) {
            return true;
        }

        return false;

    }
}
