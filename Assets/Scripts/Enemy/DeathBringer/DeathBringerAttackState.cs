using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private BossDeathBringer enemy;
    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = enemy;
    }

    public override void Enter() {
        base.Enter();
        AudioManager.instance.PlaySFX(28, enemy.transform);
        enemy.chanceToTeleport += 10;
    }

    public override void Exit() {
        base.Exit();
        enemy.lastTimeAttack = Time.time;
    }

    public override void Update() {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled) {
            if(enemy.CanTeleport())
                stateMachine.ChangeState(enemy.teleportState);
            else
                stateMachine.ChangeState(enemy.battleState);

        }
    }
}
