using UnityEngine;

public class PlayerCounterAttackState : PlayerState {
    private bool cancreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
    }

    public override void Enter() {
        base.Enter();

        cancreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                if (hit.GetComponent<Enemy>().CanBeStunned()) {
                    stateTimer = 10;
                    player.anim.SetBool("SuccessfulCounterAttack", true);

                    player.skill.parry.UseSkill(); //dung cho restore parry

                    if(cancreateClone) {
                        cancreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }

                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
}