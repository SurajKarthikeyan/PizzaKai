using UnityEngine;

/// <summary>
/// A simple interface that connects with <see cref="CharacterMovementModule"/> to
/// allow the player to control their character. It also allows for easy
/// integration with the new input system.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(CharacterMovementModule))]
public class PlayerControlModule : Module
{
    #region Variables
    public CharacterMovementModule movementController;

    public WeaponMasterModule weaponMaster;
    #endregion

    #region Instantiation
    private void OnValidate()
    {
        SetComponents();
    }

    private void Awake()
    {
        SetComponents();
    }

    private void SetComponents()
    {
        this.AutofillComponent(ref movementController);
        
        if (!weaponMaster)
            this.RequireComponentInChildren(out weaponMaster);
    }
    #endregion

    #region Main Loop
    private void Update()
    {
        movementController.inputtedMovement = new(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        movementController.inputtedJump = Input.GetButton("Jump");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementController.inputtedDash =
                movementController.inputtedMovement;
        }
        else
        {
            movementController.inputtedDash = Vector2.zero;
        }
    }
    #endregion
}