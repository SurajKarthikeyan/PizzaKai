using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages layers.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class LayersManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to an LayersManager instance.
    /// </summary>
    private static LayersManager instance;

    /// <summary>
    /// The static reference to an LayersManager instance.
    /// </summary>
    public static LayersManager Instance => instance;
    #endregion

    private void Start()
    {
        
    }

    #region Structs
    [System.Serializable]
    public struct DefinedLayer : IEquatable<DefinedLayer>, ISerializationCallbackReceiver
    {
        #region Variables
        [SerializeField]
        private string name;
        
        [SerializeField]
        private int layer;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                name ??= LayerMask.LayerToName(layer);

                return name;
            }
        }

        public int Layer
        {
            get
            {
                if (layer < 0 && name != "[Invalid]")
                    layer = LayerMask.NameToLayer(name);

                return layer;
            }
        }
        #endregion

        #region Constructors
        public DefinedLayer(string name, int layer)
        {
            this.name = name;
            this.layer = layer;
        }

        public DefinedLayer(string name)
        {
            this.name = name;
            this.layer = -1;
        }

        public DefinedLayer(int layer)
        {
            this.name = null;
            this.layer = layer;
        }
        #endregion

        #region Operators
        public static implicit operator int(DefinedLayer definedLayer) => definedLayer.Layer;
        public static implicit operator string(DefinedLayer definedLayer) => definedLayer.Name;
        public static bool operator ==(DefinedLayer left, DefinedLayer right) => left.Equals(right);
        public static bool operator !=(DefinedLayer left, DefinedLayer right) => !left.Equals(right);
        #endregion

        #region Methods
        public bool Equals(DefinedLayer other)
        {
            return this.layer == other.layer;
        }

        public override bool Equals(object other)
        {
            if (other is DefinedLayer defined)
                return Equals(other);
            else if (other is int layer)
                return this.layer == layer;

            return false;
        }

        public override int GetHashCode()
        {
            return layer.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} ({Layer})";
        }

        public void OnBeforeSerialize()
        {
            name = Name;
            layer = Layer;
        }

        public void OnAfterDeserialize()
        {
            // Nothing needs to be done here.
        }
        #endregion
    }
    #endregion

    #region Variables
    public static readonly DefinedLayer Invalid = new("[Invalid]", -1);

    public static readonly DefinedLayer Default = new("Default");

    public static readonly DefinedLayer TransparentFX = new("TransparentFX");

    public static readonly DefinedLayer IgnoreRaycast = new("Ignore Raycast");

    public static readonly DefinedLayer Ground = new("Ground");

    public static readonly DefinedLayer Water = new("Water");

    public static readonly DefinedLayer UI = new("UI");

    public static readonly DefinedLayer Platform = new("Platform");

    public static readonly DefinedLayer Player = new("Player");

    public static readonly DefinedLayer Box = new("Box");

    public static readonly DefinedLayer EnemyProjectile = new("EnemyProjectile");

    public static readonly DefinedLayer Enemy = new("Enemy");

    public static readonly DefinedLayer Breadstick = new("Breadstick");

    public static readonly DefinedLayer Boundary = new("Boundary");

    public static readonly DefinedLayer Background = new("Background");

    public IEnumerable<Collider2D> platformColliders;
    #endregion

    #region Instantiation
    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
        platformColliders = GameObject.FindObjectsOfType<Collider2D>()
            .Where(c => c.gameObject.layer == Platform);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Moves all children of root into the specified layer.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="layer"></param>
    public static void MoveAllToLayer(GameObject root, int layer)
    {
        root.layer = layer;

        foreach (Transform child in root.transform)
        {
            MoveAllToLayer(child.gameObject, layer);
        }
    }

    /// <summary>
    /// Returns a layer mask from <paramref name="layers"/>.
    /// </summary>
    /// <param name="layers">The layers to exclude from the mask.</param>
    /// <returns></returns>
    public static LayerMask GetMask(params int[] layers)
    {
        int mask = 0;

        foreach (var layer in layers)
        {
            mask |= 1 << layer;
        }

        return mask;
    }

    public void IgnoreCollisionsWithPlatforms(Collider2D collider,
        bool ignore)
    {
        foreach (var platformCollider in platformColliders)
        {
            if (platformCollider != null) 
            {
                Physics2D.IgnoreCollision(platformCollider, collider, ignore);
            }
        }
    }
    #endregion
}