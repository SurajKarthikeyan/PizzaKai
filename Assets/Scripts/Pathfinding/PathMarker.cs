using UnityEngine;

/// <summary>
/// Marks certain nodes, advertising them for the <see
/// cref="PathfindingManager"/>.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class PathMarker : MonoBehaviour
{
    #region Enums
    /// <summary>
    /// How "solid" this tile is. Affects how the tile is treated when it's
    /// added to the graph, as well as how its neighbors are treated.
    /// </summary>
    [System.Serializable]
    public enum Solidity
    {
        /// <summary>
        /// This marker has no effect on the path or AI navigation.
        /// </summary>
        None,
        /// <summary>
        /// AI can walk on this tile and cannot move through it.
        /// </summary>
        Solid,
        /// <summary>
        /// AI can walk on this tile, can move through the tile horizontally (if
        /// there is ground underneath), and can pass through the tile from
        /// below and above. Use the <see cref="DirectionData"/> to restrict
        /// movement in a certain direction (for example, to create a one-way
        /// platform).
        /// </summary>
        Platform,
    }
    #endregion

    #region Structs
    /// <summary>
    /// Store this marker's info in a struct, since that's the easiest way to
    /// ensure that it get serialized correctly.
    /// </summary>
    [System.Serializable]
    public struct MarkerData
    {
        #region Main Data
        /// <summary>
        /// The heuristic cost of moving into this tile.
        /// </summary>
        public float heuristic;

        /// <summary>
        /// How does this tile affect AI path traversal?
        /// </summary>
        public Solidity solidity;

        /// <summary>
        /// Determines how the AI should move into or out of tiles.
        /// </summary>
        public TileMovementData movementData;
        #endregion
    }

    [System.Serializable]
    public struct TileMovementData
    {
        public DirectionData north;
        public DirectionData northEast;
        public DirectionData east;
        public DirectionData southEast;
        public DirectionData south;
        public DirectionData southWest;
        public DirectionData west;
        public DirectionData northWest;
    }

    /// <summary>
    /// Defines metadata for a certain movement direction (both incoming and
    /// outgoing).
    /// </summary>
    [System.Serializable]
    public struct DirectionData
    {
        #region Enums
        /// <summary>
        /// Determines which path (ingoing or outgoing) is blocked.
        /// </summary>
        public enum Traversability
        {
            /// <summary>
            /// Marks this direction as traversable, so AIs can move into and
            /// out of the tile square. This is overridden if the neighboring
            /// tile has its traversability set to <see cref="OneWay"/> or <see
            /// cref="Blocked"/>.
            /// </summary>
            Open,
            /// <summary>
            /// Prevents AI from moving out from this tile, but does nothing to
            /// AI moving into it.
            /// </summary>
            OneWay,
            /// <summary>
            /// Prevents AI from both moving into and out of this tile.
            /// </summary>
            Blocked
        }
        #endregion

        #region Variables
        public Traversability traversability;
        #endregion
    }
    #endregion

    #region Variables
    /// <summary>
    /// The data for this marker.
    /// </summary>
    public MarkerData markerData;
    #endregion
}