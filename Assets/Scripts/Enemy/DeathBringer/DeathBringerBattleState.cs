using UnityEngine;

public class DeathBringerBattleState : EnemyState {
    private Transform player;
    private BossDeathBringer enemy;
    private int moveDir;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        //if (player.GetComponent<PlayerStats>().isDead)
        //    stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if (enemy.isPlayerDetected()) {
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance) {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else 
                    stateMachine.ChangeState(enemy.idleState);
            }
        }

        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);
        if (distanceToPlayerX < 1)
            stateMachine.ChangeState(enemy.attackState);

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackDistance - .1f)
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
