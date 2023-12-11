using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAnimationEvents : MonoBehaviour
{
    public CharacterMovementModule playerCharacterMovementModule;
    public ShotGunWeapon playerShotGunWeapon;
    public SpriteRenderer shotgunSpriteRenderer;
    public GameObject debris;
    public Transform debrisSpawnpoint;

    public Material playerMat;
    public Texture2D glowTexDashStart;
    public Texture2D glowTexDashSlam;
    public Texture2D playerDefault;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");
    public void DisableInputs()
    {
        playerMat.SetTexture(GlowTex, glowTexDashStart);
        playerCharacterMovementModule.canInput = false;
        playerShotGunWeapon.shotgunDashing = true;
        shotgunSpriteRenderer.enabled = false;
    }

    public void SetSlamEmission()
    {
        playerMat.SetTexture(GlowTex, glowTexDashSlam);
    }

    public void RemoveGlowTex()
    {
        playerMat.SetTexture(GlowTex, playerDefault);
    }

    public void EnableInput()
    {
        playerCharacterMovementModule.canInput = true;
        playerShotGunWeapon.shotgunDashing = false;
        shotgunSpriteRenderer.enabled = true;
        playerMat.SetTexture(GlowTex, playerDefault);
    }
    public void SpawnDebris()
    {
        GameObject _debris = Instantiate(debris);
        _debris.transform.position = debrisSpawnpoint.position;
    }
}
