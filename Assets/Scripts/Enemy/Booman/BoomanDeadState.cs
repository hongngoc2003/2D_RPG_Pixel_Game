using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomanDeadState : EnemyState
{
    private EnemyBooman enemy;
    public BoomanDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBooman _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Update() {
        base.Update();

        if(triggerCalled)
            enemy.SelfDestroy();
    }
}
