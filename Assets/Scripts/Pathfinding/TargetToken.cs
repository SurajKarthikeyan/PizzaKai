using System;
using UnityEngine;

/// <summary>
/// Contains basic information about the a target on a path. Can either be a
/// specific tile or a dynamic transform.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[Serializable]
public class TargetToken : IEquatable<TargetToken>
{
    #region Member Variables
    /// <summary>
    /// Tracking a static target.
    /// </summary>
    private readonly Vector3 staticPosition;

    /// <summary>
    /// Tracking a dynamic target.
    /// </summary>
    private readonly Transform dynamicTarget;
    #endregion

    #region Variables
    public Vector3 offset = Vector3.zero;
    #endregion

    #region Properties
    /// <summary>
    /// The target position in world coordinates.
    /// </summary>
    public Vector3 Position => (IsDynamic ? dynamicTarget.position : staticPosition) + offset;

    /// <summary>
    /// The target position in grid coordinates.
    /// </summary>
    public Vector3Int GridPosition => PathfindingManager.Instance.WorldToCell(Position);

    /// <summary>
    /// True if tracking a dynamic transform.
    /// </summary>
    public bool IsDynamic => dynamicTarget;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a path token targeting a grid tile.
    /// </summary>
    /// <param name="gridPosition">The grid positions of the tile.</param>
    public TargetToken(Vector3Int gridPosition)
    {
        staticPosition = PathfindingManager.Instance.CellToWorld(gridPosition);
    }

    /// <summary>
    /// Creates a path token targeting a specific world location.
    /// </summary>
    /// <param name="worldPosition"></param>
    public TargetToken(Vector3 worldPosition)
    {
        staticPosition = worldPosition;
    }

    /// <summary>
    /// Creates a path token targeting some transform.
    /// </summary>
    /// <param name="tracking">The transform to track.</param>
    public TargetToken(Transform tracking)
    {
        dynamicTarget = tracking;
    }
    #endregion

    #region Getters/Setters
    public Vector3 GetHeading(Vector3 currentLocation)
    {
        return Position - currentLocation;
    }
    #endregion

    #region Object Overrides
    public override string ToString()
    {
        return (IsDynamic ? "Dynamic" : "Fixed") + 
            $" [at {Position}, grid {GridPosition}" +
            (IsDynamic ? $", tracking {dynamicTarget}]" : "]");
    }

    public override bool Equals(object obj)
    {
        return obj is TargetToken tt && Equals(tt);
    }

    public bool Equals(TargetToken other)
    {
        if (IsDynamic != other.IsDynamic)
            return false;
        else
        {
            if (IsDynamic)
            {
                // Make sure dynamic targets match up.
                return dynamicTarget == other.dynamicTarget;
            }
            else
            {
                // Make sure static targets match up.
                return staticPosition == other.staticPosition;
            }
        }
    }

    public override int GetHashCode()
    {
        if (IsDynamic)
            return HashCode.Combine(IsDynamic, dynamicTarget);
        else
            return HashCode.Combine(staticPosition, IsDynamic);
    }
    #endregion
}