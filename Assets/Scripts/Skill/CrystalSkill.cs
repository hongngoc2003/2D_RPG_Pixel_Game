using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill {
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Simple crystal")]
    [SerializeField]private UISkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked {  get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private UISkillTreeSlot unlockCrystalTurnIntoCloneButton;
    [SerializeField] private bool crystalTurnIntoClone;

    [Header("Explosive crystal")]
    [SerializeField] private UISkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private UISkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Stack crystal")]
    [SerializeField] private UISkillTreeSlot unlockMultiCrystalButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStack;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    protected override void Start() {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalTurnIntoCloneButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStackCrystal);

    }

    #region Unlock Skill

    protected override void CheckUnlock() {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMultiStackCrystal();
        UnlockMovingCrystal();
    }
    private void UnlockCrystal() {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }
    private void UnlockCrystalMirage() {
        if (unlockCrystalTurnIntoCloneButton.unlocked)
            crystalTurnIntoClone = true;
    }
    private void UnlockExplosiveCrystal() {
        if(unlockExplosiveButton.unlocked)
            canExplode = true;
    }
    private void UnlockMovingCrystal() {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }
    private void UnlockMultiStackCrystal() {
        if(unlockMultiCrystalButton.unlocked)
            canUseMultiStack = true;
    }
    #endregion
    public override void UseSkill() {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null) {
            CreateCrystal();
        } else {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(crystalTurnIntoClone) {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            } else {
                currentCrystal.GetComponent<CrystalSkillController>()?.CrystalLogic();

            }
            
        }
    }

    public void CreateCrystal() {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() {
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }
    public bool CanUseMultiCrystal() {
        if(canUseMultiStack) {
            if(crystalLeft.Count > 0) {

                if (crystalLeft.Count == amountOfStack)
                    Invoke("ResetAbility", useTimeWindow);

                coolDown = 0;

                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().
                    SetupCrystal(crystalDuration, canExplode,canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if(crystalLeft.Count <= 0) {
                    coolDown = multiStackCooldown;
                    RefillCrystal();
                }

                return true;
            }

        }
        return false;
    }

    private void RefillCrystal() {
        int amountToAdd = amountOfStack - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++) {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility() {
        if(coolDownTimer > 0)
            return;

        coolDownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
