using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using static LayersManager;
using static PathMarker;

/// <summary>
/// Contains info about a vertex in the pathfinding graph. Each tilemap can
/// create up to one PathNode per cell in the grid.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class PathNode : IEquatable<PathNode>
{
    #region Enum
    [System.Flags]
    public enum NodeFlags
    {
        /// <summary>
        /// There is nothing in this node.
        /// </summary>
        None = 0,
        /// <summary>
        /// If this node exists (its tile is not null).
        /// </summary>
        Exists = 1 << 0,
        /// <summary>
        /// Set if this node contains a defined game object.
        /// </summary>
        HasGameObject = 1 << 2,
        /// <summary>
        /// Set if the game object contains a <see cref="PathMarker"/>.
        /// </summary>
        HasMarker = 1 << 3,
        /// <summary>
        /// Set if the tile directly under this node is considered to be
        /// navigable and solid.
        /// </summary>
        WalkableOnSolid = 1 << 4,
        /// <summary>
        /// Set if the tile directly under this node is considered to be
        /// navigable and a platform.
        /// </summary>
        WalkableOnPlatform = 1 << 5,
        /// <summary>
        /// Set if the tile directly under this node is considered to be
        /// navigable.
        /// </summary>
        Walkable = WalkableOnSolid | WalkableOnPlatform,
    }
    #endregion

    #region Variables
    #region Readonly and Const
    public static readonly LayerMask groundMask = LayerExt.CreateMask(
        LayersManager.Platform,
        LayersManager.Ground
    );
    #endregion

    /// <summary>
    /// The grid coordinates of this thing.
    /// </summary>
    public Vector3Int gridPosition;

    public NodeFlags flags;

    /// <summary>
    /// The layer of the tilemap.
    /// </summary>
    public DefinedLayer mapLayer;

    public Tile.ColliderType colliderType;

    /// <summary>
    /// The data of the path marker attached to the game object, if one exists.
    /// Otherwise, this is set to its default value.
    /// </summary>
    public PathMarker.MarkerData markerData;

    public string tilemapName;

    public int tilemapID;
    #endregion

    #region Properties
    /// <summary>
    /// Whether or not this node represents a solid block, a platform, or
    /// something that has no effect on movement.
    /// </summary>
    public Solidity Solidity
    {
        get
        {
            if (flags.HasFlag(NodeFlags.HasMarker))
                return markerData.solidity;
            else if (colliderType == Tile.ColliderType.None)
                return Solidity.None;
            else if (flags.HasFlag(NodeFlags.Exists))
            {
                if (mapLayer == LayersManager.Platform)
                    return Solidity.Platform;
                else if (mapLayer == LayersManager.Ground)
                    return Solidity.Solid;
                else if (groundMask.ContainsLayer(mapLayer))
                    return Solidity.Solid;
            }

            return Solidity.None;
        }
    }

    /// <summary>
    /// If true, then this node can be walked on.
    /// </summary>
    public bool CanWalkOn => Solidity switch
    {
        Solidity.Solid or Solidity.Platform => true,
        _ => false
    };

    /// <summary>
    /// If true, then this node can be walked through (it's not solid).
    /// </summary>
    public bool CanWalkThrough => Solidity switch
    {
        Solidity.None or Solidity.Platform => true,
        _ => false
    };
    #endregion

    #region Constructors
    /// <summary>
    /// Generates a new <see cref="PathNode"/> using the provided tilemap.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="tilemap"></param>
    public PathNode(Vector3Int gridPosition, Tilemap tilemap)
    {
        this.gridPosition = gridPosition;

        if (tilemap)
        {
            tilemapName = tilemap.gameObject.name;
            tilemapID = tilemap.GetInstanceID();
    
            var tile = tilemap.GetTile(gridPosition);
            mapLayer = new(tilemap.gameObject.layer);
            colliderType = tilemap.GetColliderType(gridPosition);
    
            // Set default value of path marker here. Right now, the default
            // defaults are good, but if that ever changes, put the updated code
            // here.
    
            if (tile)
            {
                // Debug.Log(tile);
    
                flags |= NodeFlags.Exists;
    
                var gameObj = tilemap.GetInstantiatedObject(gridPosition);
    
                if (gameObj)
                {
                    flags |= NodeFlags.HasGameObject;
    
                    if (gameObj.HasComponent(out PathMarker marker))
                    {
                        flags |= NodeFlags.HasMarker;
                        markerData = marker.markerData;
                    }
                }
            }
        }
        else
        {
            tilemapName = "[None]";
            tilemapID = -1;

            mapLayer = LayersManager.Invalid;
        }
    }
    #endregion

    #region Methods
    #region IEquatable
    public bool Equals(PathNode other)
    {
        return this.gridPosition == other.gridPosition &&
            this.mapLayer.Equals(other.mapLayer) &&
            this.flags == other.flags;
    }
    #endregion

    #region ToString
    public override string ToString()
    {
        return $"Node [at {gridPosition}, " +
            $"flags {flags}, " +
            $"map layer {mapLayer}, " +
            $"collider type {colliderType}, " +
            $"marker data {markerData}]";
    } 
    #endregion
    #endregion
}