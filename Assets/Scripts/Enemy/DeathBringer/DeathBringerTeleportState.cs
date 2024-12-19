using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState {
    private BossDeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        enemy.stats.MakeInvicible(true);
    }

    public override void Update() {
        base.Update();

        if (triggerCalled) {
            if(enemy.CanDoSpellCast()) 
                stateMachine.ChangeState(enemy.spellCastState);
            else 
                stateMachine.ChangeState(enemy.battleState);
        }
            
    }

    public override void Exit() {
        base.Exit();

        enemy.stats.MakeInvicible(false);
    }
}
