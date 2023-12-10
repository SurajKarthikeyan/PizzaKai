using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAnimationEvents : MonoBehaviour
{
    public CharacterMovementModule playerCharacterMovementModule;
    public void DisableInputs()
    {
        playerCharacterMovementModule.canInput = false;
    }

    public void EnableInput()
    {
        playerCharacterMovementModule.canInput = true;
    }
}
