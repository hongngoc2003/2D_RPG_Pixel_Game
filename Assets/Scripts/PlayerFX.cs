using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen shake fx")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDmg;

    protected override void Start() {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake(Vector3 _shakePower) {
        screenShake.m_DefaultVelocity = new Vector3
            (_shakePower.x * PlayerManager.instance.player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

}
