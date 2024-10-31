using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState {
    private EnemySkeleton enemy;
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemy, _stateMachine, _animBoolName) {
        enemy = _enemy;
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
        Debug.Log(stateTimer);

        if(stateTimer < 0) 
            stateMachine.ChangeState(enemy.moveState);
    }
}
