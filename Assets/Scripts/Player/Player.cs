﻿using System.Collections;
using UnityEngine;

public class Player : Entity {

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float doubleJumpForce;
    public bool canDoubleJump;

    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;


    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFX fx { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDoubleJumpState doubleJumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackHoleState blackHole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake() {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        doubleJumpState = new PlayerDoubleJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");


        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }

    protected override void Start() {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        fx = GetComponent<PlayerFX>();
    }


    protected override void Update() {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if (UserInput.instance.crystalInput && skill.crystal.crystalUnlocked) {
            skill.crystal.CanUseSkill();
        }

        if (UserInput.instance.flaskInput) {
            Inventory.instance.UseFlask();
        }
    }
    public override void SlowEntityBy(float _slowPercent, float _slowDuration) {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed() {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword) {
        sword = _newSword;
    }
    public void CatchSword() {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    public IEnumerator BusyFor(float _seconds) {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput() {
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;

        if (UserInput.instance.dashInput && SkillManager.instance.dash.CanUseSkill()) {
            dashDir = UserInput.instance.moveInput.x;

            if (dashDir == 0)
                dashDir = facingDir;


            stateMachine.ChangeState(dashState);
        }
    }

    protected override void SetupZeroKnockbackPower() {
        knockbackPower = new Vector2(0, 0);
    }
    public void ResetDoubleJump() {
        canDoubleJump = true; // Reset Double Jump khi nhân vật chạm đất
    }

    public override void Die() {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
