using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathBringer : Enemy
{
    public bool bossFightBegin;

    [Header("Spell cast details")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpell;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    

    [Header("Teleport details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 aroundCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;

 
    #region States

    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    #endregion


    protected override void Awake() {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start() {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void Die() {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public void CastSpell() {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        if(player.rb.velocity.x != 0)
            xOffset = player.facingDir * 2;

        Vector3 spellPosition = new Vector3
            (player.transform.position.x + xOffset, player.transform.position.y + 1.5f);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);

        newSpell.GetComponent<DeathBringerSpellController>().SetupSpell(stats);
    }

    public void FindPosition() {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x -3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y -3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (capsuleCol.size.y)/2);

        if(!GroundBelow() || IsSomethingAround()) {
            Debug.Log("Looking for new position");
            FindPosition();
        }
    } 

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool IsSomethingAround() => Physics2D.BoxCast(transform.position, aroundCheckSize, 0, Vector2.zero,0,whatIsGround);


    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.DrawLine (transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, aroundCheckSize);
    }

    public bool CanTeleport() {
        if(Random.Range(0,100) <= chanceToTeleport) {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast() {
        if (Time.time >= lastTimeCast + spellStateCooldown ) {
            return true;
        }
        return false;
    }
}
