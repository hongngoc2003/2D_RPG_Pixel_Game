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
    [SerializeField] private int attackCooldown;

    public override bool CanUseSkill() {
        return base.CanUseSkill();
    }

    public override void UseSkill() {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab);

        BlackHoleSkillController newBlackHoleScript = newBlackHole.GetComponent<BlackHoleSkillController>();

        newBlackHoleScript.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, attackCooldown);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }
}
