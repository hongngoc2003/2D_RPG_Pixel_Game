using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private CharacterStats myStats;
    private Slider slider;
    private void Start() {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        slider.maxValue = myStats.GetFullHealthValue();
        slider.value = myStats.currentHealth;
    }
    private void FlipUI() {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable() {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
