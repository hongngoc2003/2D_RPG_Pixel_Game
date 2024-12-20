using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill {
    [Header("Dodge")]
    [SerializeField] private UISkillTreeSlot unlockDodgeButton;
    public bool dodgeUnlocked;
    [SerializeField] private int evasionAmount;

    [Header("Mirage dodge")]
    [SerializeField] private UISkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start() {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    protected override void CheckUnlock() {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge() {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked) {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirageDodge() {
        if(unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge() {
        if (dodgeMirageUnlocked) {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
        }
    }
}
