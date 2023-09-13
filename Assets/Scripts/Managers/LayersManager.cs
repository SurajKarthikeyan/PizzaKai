using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    #region Structs
    [Serializable]
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

        // public DefinedLayer(string name)
        // {
        //     this.name = name;
        //     this.layer = -1;
        // }

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
    #region Defined Layers
    public static readonly DefinedLayer Invalid = new("[Invalid]", -1);

    public static readonly DefinedLayer Default = new("Default", 0);

    public static readonly DefinedLayer TransparentFX = new("TransparentFX", 1);

    public static readonly DefinedLayer IgnoreRaycast = new("Ignore Raycast", 2);

    public static readonly DefinedLayer Ground = new("Ground", 3);

    public static readonly DefinedLayer Water = new("Water", 4);

    public static readonly DefinedLayer UI = new("UI", 5);

    public static readonly DefinedLayer Platform = new("Platform", 6);

    public static readonly DefinedLayer Player = new("Player", 7);

    public static readonly DefinedLayer Box = new("Box", 8);

    public static readonly DefinedLayer EnemyProjectile = new("EnemyProjectile", 9);

    public static readonly DefinedLayer Enemy = new("Enemy", 10);

    public static readonly DefinedLayer Breadstick = new("Breadstick", 11);

    public static readonly DefinedLayer Boundary = new("Boundary", 12);

    public static readonly DefinedLayer Background = new("Background", 13);

    #endregion

    public List<Collider2D> platformColliders;

    private HashSet<Collider2D> ignorePlatformColliders = new();

    private IEnumerator csipc_CR;
    #endregion

    #region Instantiation
    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
        platformColliders = GameObject.FindObjectsOfType<Collider2D>()
            .Where(c => c.gameObject.layer == Platform)
            .ToList();

        ignorePlatformColliders = new();
    }

    private void OnEnable()
    {
        if (csipc_CR != null)
        {
            csipc_CR = ClearStaleIPC_CR();
            StartCoroutine(csipc_CR);
        }
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
        // Add to the platforms.
        if (ignore)
        {
            if (ignorePlatformColliders.Contains(collider))
                return;
            
            ignorePlatformColliders.Add(collider);
        }
        else
        {
            ignorePlatformColliders.Remove(collider);
        }

        foreach (var platformCollider in platformColliders)
        {
            if (platformCollider != null)
            {
                Physics2D.IgnoreCollision(platformCollider, collider, ignore);
            }
        }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Periodically clears null/destroyed colliders from the <see
    /// cref="ignorePlatformColliders"/>.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClearStaleIPC_CR()
    {
        while (enabled)
        {
            yield return new WaitForSecondsRealtime(30);

            ignorePlatformColliders
                .RemoveWhere(c => !c);
        }
    }
    #endregion
}