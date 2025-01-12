using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;


    [Header("Soul info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    private float increaseRate = 10000;


    private void Start() {
        if(playerStats != null) {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }
    private void Update() {
        UpdateSoulsUI();

        if (UserInput.instance.dashInput && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (UserInput.instance.parryInput && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (UserInput.instance.crystalInput && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (UserInput.instance.aimInput && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (UserInput.instance.blackholeInput && skills.blackHole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if (UserInput.instance.flaskInput && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(parryImage, skills.parry.coolDown);
        CheckCooldownOf(crystalImage, skills.crystal.coolDown);
        CheckCooldownOf(swordImage, skills.sword.coolDown);
        CheckCooldownOf(blackholeImage, skills.blackHole.coolDown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI() {
        if (soulsAmount < PlayerManager.instance.GetCurrency()) {
            soulsAmount += Time.deltaTime * increaseRate;
        } else {
            soulsAmount = PlayerManager.instance.GetCurrency();
        }

        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI() {
        slider.maxValue = playerStats.GetFullHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image) {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _coolDown) {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _coolDown * Time.deltaTime;
    }
}
