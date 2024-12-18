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
        enemy.FindPosition();
        stateTimer = 1;
    }

    public override void Update() {
        base.Update();

        if(stateTimer < 0) {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
