using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour {
    private SpriteRenderer sr;

    [Header("Screen shake fx")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDmg;
    

    [Header("FlashFX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] freezeColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particle")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake(Vector3 _shakePower) {
        screenShake.m_DefaultVelocity = new Vector3
            (_shakePower.x * PlayerManager.instance.player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }
    public void MakeTransparent(bool _transparent) {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    private IEnumerator FlashFX() {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        
        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }
    private void RedColorBlink() {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelColorChange() {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }
    public void IgniteFXFor(float _seconds) {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void FreezeFXFor(float _seconds) {
        chillFX.Play();

        InvokeRepeating("FreezeColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFXFor(float _seconds) {
        shockFX.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx() {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else 
            sr.color = igniteColor[1];
    }
    private void ShockColorFx() {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    private void FreezeColorFx() {
        if (sr.color != freezeColor[0])
            sr.color = freezeColor[0];
        else
            sr.color = freezeColor[1];
    }

    public void CreateCriticalHitFX(Transform _target) {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        GameObject newhitFX = Instantiate(hitFX, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        newhitFX.transform.Rotate(new Vector3(0, 0, zRotation));

        Destroy(newhitFX, .5f);
    }
}
