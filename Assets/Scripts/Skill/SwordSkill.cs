using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType {
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private UISkillTreeSlot bounceUnlockButton;
    [SerializeField] private int amountBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private UISkillTreeSlot pierceUnlockButton;
    [SerializeField] private int amountPierce;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UISkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCoolDown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;


    [Header("Skill info")]
    [SerializeField] private UISkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetwwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    private GameObject[] dots;

    [Header("Passive skills")]
    [SerializeField] private UISkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked {  get; private set; }
    [SerializeField] private UISkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    protected override void Start() {
        base.Start();

        GenerateDots();

        SetUpGravity();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
    }
    private void SetUpGravity() {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }
    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        
        if(Input.GetKey(KeyCode.Mouse1)) {
            for (int i = 0; i < dots.Length; i++) {
                dots[i].transform.position = DotsPosition(i * spaceBetwwenDots);
            }
        }    
    
    }
    public void CreateSword() {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, amountBounce, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(amountPierce);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCoolDown);


        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);

        AudioManager.instance.PlaySFX(27,null);
    }
    #region Aim region
    public Vector2 AimDirection() {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }
    public void DotsActive(bool _isActive) {
        for (int i = 0; i < dots.Length; i++) {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots() {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++) {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t) {
        Vector2 position = (Vector2)player.transform.position +
            new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y)
            * t + .5f * (Physics2D.gravity * swordGravity) * (t * t); //phuong trinh chuyen dong equation of motion

        return position;
    }
    #endregion

    #region Unlock region

    protected override void CheckUnlock() {
        UnlockSword();
        UnlockBounce();
        UnlockSpin();
        UnlockPierce();
        UnlockTimeStop();
        UnlockVulnerable();
    }
    private void UnlockTimeStop() {
        if(timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }
    private void UnlockVulnerable() {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }
    private void UnlockSword() {
        if (swordUnlockButton.unlocked) {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }
    private void UnlockBounce() {
        if(bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierce() {
        if(pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpin() {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }
    #endregion
}
