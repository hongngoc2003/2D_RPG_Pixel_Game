using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomanBattleState : EnemyState
{
    private Transform player;
    private EnemyBooman enemy;
    private int moveDir;

    private float defaultSpeed;  

    public BoomanBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBooman _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        defaultSpeed = enemy.moveSpeed;

        enemy.moveSpeed = enemy.battleStateMoveSpeed;

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit() {
        base.Exit();

        enemy.moveSpeed = defaultSpeed; 
    }

    public override void Update() {
        base.Update();

        if (enemy.isPlayerDetected()) {
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance) {
                enemy.stats.KillEntity();
            }
        } else {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackDistance - 0.5f)
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public bool CanAttack() {
        if (Time.time >= enemy.lastTimeAttack + enemy.attackCooldown) {
            enemy.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }
}
