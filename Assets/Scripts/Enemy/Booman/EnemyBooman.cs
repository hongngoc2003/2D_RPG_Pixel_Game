using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyBooman : Enemy
{
    [Header("Booman Specific")]
    public float battleStateMoveSpeed;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region States
    public BoomanIdleState idleState { get; private set; }
    public BoomanMoveState moveState { get; private set; }
    public BoomanBattleState battleState { get; private set; }
    public BoomanStunnedState stunnedState { get; private set; }
    public BoomanDeadState deadState { get; private set; }
    //public BoomanRunState runState { get; private set; }
    #endregion
    protected override void Awake() {
        base.Awake();

        idleState = new BoomanIdleState(this, stateMachine, "Idle", this);
        moveState = new BoomanMoveState(this, stateMachine, "Move", this);
        battleState = new BoomanBattleState(this, stateMachine, "Move", this);
        stunnedState = new BoomanStunnedState(this, stateMachine, "Stunned", this);
        deadState = new BoomanDeadState(this, stateMachine, "Dead", this);
        //runState = new BoomanRunState(this, stateMachine, "Run", this);

    }
    protected override void Start() {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned() {
        if (base.CanBeStunned()) {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
    public override void Die() {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger() {
        GameObject newExplosion = Instantiate(explosionPrefab, attackCheck.position, Quaternion.identity);

        newExplosion.GetComponent<BoomanExplosionController>().SetupExplosion(stats, growSpeed, maxSize, attackCheckRadius);

        capsuleCol.enabled = false;
        rb.gravityScale = 0;
    }
    public void SelfDestroy() => Destroy(gameObject);

}
