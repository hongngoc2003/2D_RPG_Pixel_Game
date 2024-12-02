using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UISkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UISkillTreeSlot parryRestoreUnlockButton;
    public bool parryRestoreUnlocked {  get; private set; }
    [Range(0,1)]
    [SerializeField] private float restoreHealthPercent;

    [Header("Restore with mirage")]
    [SerializeField] private UISkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked {  get; private set; }

    public override void UseSkill() {
        base.UseSkill();

        if(parryRestoreUnlocked) {
            int restoreHealthAmount = Mathf.RoundToInt(player.stats.GetFullHealthValue() * restoreHealthPercent); 
            player.stats.IncreaseHealthBy(restoreHealthAmount);
        }
    }

    protected override void Start() {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockRestoreParry);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }
    protected override void CheckUnlock() {
        UnlockParry();
        UnlockParryWithMirage();
        UnlockRestoreParry();
    }
    private void UnlockParry() {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockRestoreParry() {
        if(parryRestoreUnlockButton.unlocked)
            parryRestoreUnlocked = true;
    }

    private void UnlockParryWithMirage() {
        if(parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform) {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
