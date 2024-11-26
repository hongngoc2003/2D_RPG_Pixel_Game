using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UISkillTreeSlot dashUnlockedButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnArrivalUnlockedButton;


    protected override void Start() {
        base.Start();

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);

    }
    public override void UseSkill() {
        base.UseSkill();
    }

    private void UnlockDash() {
        if(dashUnlockedButton.unlocked) 
            dashUnlocked = true;
    }
    public void CloneOnDash() {
        if (cloneOnDashUnlocked) {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival() {
        if (cloneOnArrivalUnlocked) {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }


    private void UnlockCloneOnDash() {
        if(cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival() {
        if(cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
}
