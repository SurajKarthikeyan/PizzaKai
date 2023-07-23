using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Performs the movement of characters. Another script is required to actually
/// call the functions.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class CharacterMovement : Module
{
    #region Variables
    #region User Settings
    [InfoBox("NOTE: maxMoveSpeed.y is NOT the max jump speed! Jumping is " +
        "controlled by jumpForce.")]
    [Tooltip("The maximum move speed for this character in both horizontal " +
        "and vertical directions.")]
    public Vector2 maxMoveSpeed = new(10f, 0);

    [InfoBox("NOTE: moveAcceleration.y is NOT the jump force! Jumping is " +
        "controlled by jumpForce.")]
    [Tooltip("The move acceleration for this character in both horizontal " +
        "and vertical directions.")]
    public Vector2 moveAcceleration = new(1f, 0);

    [Tooltip("The upward force produced by jumping.")]
    public float jumpForce = 12f;

    public Duration jumpCooldown = new(0.5f);

    [Tooltip("The collider responsible for checking if the character is " +
        "grounded.")]
    public Collider2D groundCheck;
    #endregion

    #region Autogenerated
    [Tooltip("The current movement of this character. The x component " +
        "controls the horizontal movement and the vertical component " +
        "controls the vertical movement. All components are clamped " +
        "between -1 and 1.")]
    [ReadOnly]
    public Vector2 currentMovement;
    #endregion
    #endregion

    #region Methods
    #region Instantiation
    private void Start()
    {
        
        // Make sure max speed is positive.
        maxMoveSpeed = maxMoveSpeed.Abs();
    }
    #endregion

    #region Main Loop
    private void FixedUpdate()
    {
        // Here we move the character. First get the speed the character is
        // moving at and compare it to the maximal speed.
        Vector2 speed = Master.r2d.velocity.Abs();

        Vector2 force = new();

        if (speed.x <= maxMoveSpeed.x)
        {
            // The horizontal speed is less than the max horizontal speed. Allow
            // character to move.
            force.x = Mathf.Clamp(currentMovement.x, -1, 1) *
                moveAcceleration.x;
        }
        if (speed.y <= maxMoveSpeed.y)
        {
            // Ditto for max vertical speed.
            force.y = Mathf.Clamp(currentMovement.y, -1, 1) *
                moveAcceleration.y;
        }

        // Note: This is the "correct" way to change the velocity of a
        // rigidbody. Directly setting the velocity often leads to weird
        // results, such as the shotgun not being able to properly move the
        // character.
        Master.r2d.AddForce(force);

        // TODO: Fix jumping.
        // Handle jumping.
        if (jumpCooldown.IncrementFixedUpdate())
        {
            force.y += jumpForce;
        }
    }
    #endregion

    #region Public Methods
    
    #endregion

    #region Helper Methods

    #endregion
    #endregion
}