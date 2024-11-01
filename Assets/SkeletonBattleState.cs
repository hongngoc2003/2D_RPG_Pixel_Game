using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState {
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = enemy;
    }

    public override void Enter() {
        base.Enter();

        player = GameObject.Find("Player").transform;

    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if (enemy.isPlayerDetected()) {
            if (enemy.isPlayerDetected().distance < enemy.attackDistance) {
                stateMachine.ChangeState(enemy.attackState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
}
