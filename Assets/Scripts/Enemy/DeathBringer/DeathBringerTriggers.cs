using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTriggers : EnemyAnimationTriggers
{
    private BossDeathBringer deathBringer => GetComponentInParent<BossDeathBringer>();

    private void Relocate() => deathBringer.FindPosition();

    private void MakeInvisible() => deathBringer.fx.MakeTransparent(true);
    private void MakeVisible() => deathBringer.fx.MakeTransparent(false);
}
