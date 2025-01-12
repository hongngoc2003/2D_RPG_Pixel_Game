using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (UserInput.instance.blackholeInput && player.skill.blackHole.blackholeUnlocked) {
            if (player.skill.blackHole.coolDownTimer > 0)
                return;

            stateMachine.ChangeState(player.blackHole);
        }

        if (UserInput.instance.aimInput && HasNoSword() && player.skill.sword.swordUnlocked) 
            stateMachine.ChangeState(player.aimSword);

        if (UserInput.instance.parryInput && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttack);

        if (UserInput.instance.attackInput)
            stateMachine.ChangeState(player.primaryAttack);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (UserInput.instance.jumpInput && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
    private bool HasNoSword() {
        if (!player.sword)
            return true;
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
