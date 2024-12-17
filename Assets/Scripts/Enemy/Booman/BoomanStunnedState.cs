using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomanStunnedState : EnemyState
{
    private EnemyBooman enemy;
    public BoomanStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBooman _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunnedDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunnedDir.x, enemy.stunnedDir.y);
    }

    public override void Exit() {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update() {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
