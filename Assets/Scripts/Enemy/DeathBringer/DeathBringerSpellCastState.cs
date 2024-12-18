using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState {
    private BossDeathBringer enemy;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, BossDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = _enemy;
    }
}
