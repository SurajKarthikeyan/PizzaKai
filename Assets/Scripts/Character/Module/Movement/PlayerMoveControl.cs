using UnityEngine;

/// <summary>
/// A simple interface that connects with <see cref="CharacterMovement"/> to
/// allow the player to control their character. It also allows for easy
/// integration with the new input system.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class PlayerMoveControl : Module
{
    public CharacterMovement movementController;

    private void Update()
    {
        movementController.currentMovement = new(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }
}