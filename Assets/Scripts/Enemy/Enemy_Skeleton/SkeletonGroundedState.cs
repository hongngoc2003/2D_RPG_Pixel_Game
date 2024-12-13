using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState {
    protected EnemySkeleton enemy;
    protected Transform player;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) <enemy.aggroDistance)
            stateMachine.ChangeState(enemy.battleState);


    }
}
