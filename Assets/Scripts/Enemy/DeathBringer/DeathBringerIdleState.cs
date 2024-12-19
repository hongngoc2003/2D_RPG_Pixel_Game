using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private BossDeathBringer enemy;
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if (Vector2.Distance(PlayerManager.instance.player.transform.position, enemy.transform.position) < 10)
            enemy.bossFightBegin = true;

        if (Input.GetKeyDown(KeyCode.P)) {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if(stateTimer < 0 && enemy.bossFightBegin)
            stateMachine.ChangeState(enemy.battleState);
    }
}
