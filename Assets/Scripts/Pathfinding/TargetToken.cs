using UnityEngine;

/// <summary>
/// Contains basic information about the a target on a path. Can either be a
/// specific tile or a dynamic transform.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class TargetToken
{
    /// <summary>
    /// The location of the targeted tile in world coordinates.
    /// </summary>
    private readonly Vector3 tileTarget;

    /// <summary>
    /// Tracking a dynamic target.
    /// </summary>
    private readonly Transform dynamicTarget;

    /// <summary>
    /// The target position in world coordinates.
    /// </summary>
    public Vector3 Position => dynamicTarget ? dynamicTarget.position : tileTarget;

    /// <summary>
    /// The target position in grid coordinates.
    /// </summary>
    public Vector3Int GridPosition => PathfindingManager.Instance.WorldToCell(Position);

    public Vector3 GetHeading(Vector3 currentLocation)
    {
        return Position - currentLocation;
    }

    /// <summary>
    /// Creates a path token targeting a grid tile.
    /// </summary>
    /// <param name="gridPosition">The grid positions of the tile.</param>
    public TargetToken(Vector3Int gridPosition)
    {
        tileTarget = PathfindingManager.Instance.CellToWorld(gridPosition);
    }

    /// <summary>
    /// Creates a path token targeting a specific world location.
    /// </summary>
    /// <param name="worldPosition"></param>
    public TargetToken(Vector3 worldPosition)
    {
        tileTarget = worldPosition;
    }

    /// <summary>
    /// Creates a path token targeting some transform.
    /// </summary>
    /// <param name="tracking">The transform to track.</param>
    public TargetToken(Transform tracking)
    {
        dynamicTarget = tracking;
    }
}