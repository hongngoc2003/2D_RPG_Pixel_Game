using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;


    [Header("Clone attack")]
    [SerializeField] private UISkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] public bool canAttack;
    [SerializeField] private float cloneAttackMultiplier;

    [Header("Aggresive clone")]
    [SerializeField] private UISkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect {  get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private UISkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Clone Crystal")]
    [SerializeField] private UISkillTreeSlot cloneCrystalUnlockButton;
    public bool cloneCrystal;

    protected override void Start() {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        cloneCrystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneCrystal);
    }

    #region Unlock region
    protected override void CheckUnlock() {
        UnlockAggresiveClone();
        UnlockCloneAttack();
        UnlockCloneCrystal();
        UnlockMultipleClone();
    }
    private void UnlockCloneAttack() {
        if (cloneAttackUnlockButton.unlocked) {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone() {
        if (aggresiveCloneUnlockButton.unlocked) {
            canApplyOnHitEffect = true;
            attackMultiplier = aggresiveCloneAttackMultiplier;
        }
    }
    private void UnlockMultipleClone() {
        if (multipleUnlockButton.unlocked) {
            canDuplicateClone = true;
            attackMultiplier *= multipleCloneAttackMultiplier;
        }
    }
    private void UnlockCloneCrystal() {
        if (cloneCrystalUnlockButton.unlocked)
            cloneCrystal = true;
    }

    #endregion


    public void CreateClone(Transform _clonePosition, Vector3 _offset) {
        if(cloneCrystal) {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate,player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform) {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(1f * player.facingDir, 0)));
        
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset) {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
