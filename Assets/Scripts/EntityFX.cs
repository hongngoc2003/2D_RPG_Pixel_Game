using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour {
    private SpriteRenderer sr;

    [Header("Popuptext")]
    [SerializeField] private GameObject popupTextPrefab;



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

    protected virtual void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;

    }

    public void CreatePopupText(string _text) {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 5);

        Vector3 positionOffset= new Vector3(randomX,randomY,0);

        GameObject newText = Instantiate(popupTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
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
