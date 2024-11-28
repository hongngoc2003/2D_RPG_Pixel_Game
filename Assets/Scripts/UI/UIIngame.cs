using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI currentSouls;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;
    private void Start() {
        if(playerStats != null) {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }
    private void Update() {
        currentSouls.text = PlayerManager.instance.GetCurrency().ToString();

        if(Input.GetKeyDown(KeyCode.E) && skills.dash.dashUnlocked) 
            SetCooldownOf(dashImage);
        
        if(Input.GetKeyDown(KeyCode.R) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if(Input.GetKeyDown(KeyCode.Q) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (!Input.GetKeyDown(KeyCode.F) && skills.blackHole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if(!Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(parryImage, skills.parry.coolDown);
        CheckCooldownOf(crystalImage, skills.crystal.coolDown);
        CheckCooldownOf(swordImage, skills.sword.coolDown);
        CheckCooldownOf(blackholeImage, skills.blackHole.coolDown);
        CheckCooldownOf(flaskImage,Inventory.instance.flaskCooldown);
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
