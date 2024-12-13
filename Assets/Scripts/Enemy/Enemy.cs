using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]

public class Enemy : Entity {
    [Header("Stunned info")]
    public float stunnedDuration;
    public Vector2 stunnedDir;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttack;
    public float battleTime;

    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    public string lastAnimBoolName { get; private set; }


    protected override void Awake() {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start() {
        base.Start();
        fx = GetComponent<EntityFX>();
    }
    protected override void Update() {
        base.Update();

        stateMachine.currentState.Update();
    }
    public override void SlowEntityBy(float _slowPercent, float _slowDuration) {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed() {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AssignLastAnimName(string _animBoolName) {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool _timeFrozen) {
        if (_timeFrozen) {
            moveSpeed = 0f;
            anim.speed = 0f;
        } else {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezedFor(float _duration) => StartCoroutine(FreezeTimeFor(_duration));

    protected virtual IEnumerator FreezeTimeFor(float _seconds) {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }


    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow() {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow() {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned() {
        if (canBeStunned) {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}



