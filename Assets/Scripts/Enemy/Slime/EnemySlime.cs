using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big, medium, small}
public class EnemySlime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int amountSlimeCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreateVec;
    [SerializeField] private Vector2 maxCreateVec;


    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion

    protected override void Awake() {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);

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

        if (slimeType == SlimeType.small)
            return;
        CreateSlimes(amountSlimeCreate, slimePrefab);
    }

    private void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab) {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<EnemySlime>().SetupSlime(facingDir);
        }
    }

    public void SetupSlime(int _facingDir) {
        if (_facingDir != facingDir)
            Flip();

        float xVec = Random.Range(minCreateVec.x, maxCreateVec.x);
        float yVec = Random.Range(minCreateVec.y, maxCreateVec.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVec * -facingDir, yVec);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;
}
