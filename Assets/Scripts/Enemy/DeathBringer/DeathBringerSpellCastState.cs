using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState {
    private BossDeathBringer enemy;

    private int amountOfSpell;
    private float spellTimer;

    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        AudioManager.instance.PlaySFX(20, enemy.transform);


        amountOfSpell = enemy.amountOfSpell;
        spellTimer = .5f;
    }

    public override void Update() {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast()) 
            enemy.CastSpell();
         
        if(amountOfSpell <= 0) 
            stateMachine.ChangeState(enemy.teleportState);

    }
    public override void Exit() {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }
    private bool CanCast() {
        if(amountOfSpell > 0 && spellTimer < 0) {
            amountOfSpell -= 1;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}
