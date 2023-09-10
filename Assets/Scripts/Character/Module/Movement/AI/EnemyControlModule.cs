using System;
using UnityEditor.IMGUI.Controls;
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
    public CharacterMovementModule movement;

    public PathfindingAgent pathAgent;

    // public Arc sightline = new(-160, 160, 10);
    #endregion

    #region Instantiation
    private void Awake()
    {
        this.RequireComponent(out movement);
        this.RequireComponent(out pathAgent);
    }

    private void OnEnable()
    {
        // For now, just have them chase the player.
        pathAgent.SetTarget(
            GameObject.FindObjectOfType<PlayerControlModule>().transform
        );
    }
    #endregion

    #region Main Logic
    public void AcceptToken(TargetToken token)
    {
        movement.inputtedMovement = token.GetHeading(transform.position).normalized;
    }
    #endregion
}