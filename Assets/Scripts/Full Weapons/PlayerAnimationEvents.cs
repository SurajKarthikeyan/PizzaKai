using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAnimationEvents : MonoBehaviour
{
    public CharacterMovementModule playerCharacterMovementModule;
    public ShotGunWeapon playerShotGunWeapon;
    public SpriteRenderer shotgunSpriteRenderer;
    public GameObject debris;
    public Transform debrisSpawnpoint;
    public void DisableInputs()
    {
        playerCharacterMovementModule.canInput = false;
        playerShotGunWeapon.shotgunDashing = true;
        shotgunSpriteRenderer.enabled = false;
    }

    public void EnableInput()
    {
        playerCharacterMovementModule.canInput = true;
        playerShotGunWeapon.shotgunDashing = false;
        shotgunSpriteRenderer.enabled = true;
    }
    public void SpawnDebris()
    {
        GameObject _debris = Instantiate(debris);
        _debris.transform.position = debrisSpawnpoint.position;
    }
}
