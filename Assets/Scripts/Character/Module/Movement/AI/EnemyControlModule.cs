using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// This interfaces with <see cref="PathfindingAgent"/> and <see
/// cref="CharacterMovementModule"/>. It tells <see cref="PathfindingAgent"/>
/// where it wants to go, and gives that information to <see
/// cref="CharacterMovementModule"/>.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(CharacterMovementModule), typeof(PathfindingAgent))]
public class EnemyControlModule : Module
{
    #region Variables
    [ReadOnly]
    public CharacterMovementModule movement;

    [ReadOnly]
    public PathfindingAgent pathAgent;

    // public Arc sightline = new(-160, 160, 10);
    #endregion

    #region Instantiation
    private void Awake()
    {
        SetVars();
    }

    private void OnValidate()
    {
        SetVars();
    }

    private void SetVars()
    {
        this.RequireComponent(out movement);
        this.RequireComponent(out pathAgent);
    }

    private void OnEnable()
    {
        // For now, just have them chase the player.
        pathAgent.SetTarget(
            FindObjectOfType<PlayerControlModule>().transform
        );
    }
    #endregion

    #region Main Logic
    public void AcceptToken(TargetToken token)
    {
        var heading = token.GetHeading(transform.position).normalized;
        movement.inputtedMovement = heading;
        Master.SetLookAngle(Mathf.Atan2(heading.y, heading.x));
    }
    #endregion
}