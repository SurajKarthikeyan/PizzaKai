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
}
