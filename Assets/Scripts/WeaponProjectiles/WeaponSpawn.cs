using System;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Refers to a thing spawned by a weapon.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class WeaponSpawn : MonoBehaviour
{
    #region Structs
    public struct FiredByIdentifier
    {
        public readonly LayersManager.DefinedLayer layer;

        public readonly string tag;

        /// <summary>
        /// Applies only to automatic weapons (non-auto weapons have this set to
        /// 1). This signifies that this is the nth bullet of the current burst,
        /// starting from 1.
        /// </summary>
        public readonly int burstIndex;

        public readonly bool IsPlayer => tag == "Player";

        /// <summary>
        /// Creates a new identifier, in case the character is destroyed before
        /// the bullet actually hits something.
        /// </summary>
        /// <param name="weapon"></param>
        public FiredByIdentifier(WeaponModule weapon)
        {
            layer = new(weapon.Master.gameObject.layer);
            tag = weapon.Master.gameObject.tag;
            burstIndex = weapon.BurstCount;
        }
    }
    #endregion

    #region Variables
    /// <summary>
    /// What fired this spawn thingy? Make sure to check that this hasn't been
    /// destroyed.
    /// </summary>
    [ReadOnly]
    public WeaponModule firedBy;
    #endregion

    #region Methods
    /// <summary>
    /// Creates an instance of the weapon spawn and fires it.
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public WeaponSpawn Spawn(WeaponModule weapon)
    {
        // Note that [this] corresponds to the prefab of the weapon spawn, not
        // the actual instance. 
        var spawned = this.InstantiateComponent(
            weapon.firePoint.position,
            weapon.firePoint.rotation
        );

        // Set layer of spawned projectile based on if the character is an enemy
        // or player. This allows the projectile to use the default layer
        // collisions provided by LayersManager (see
        // LayersManager.playerBulletsCollision and
        // LayersManager.enemyBulletsCollision).
        spawned.gameObject.layer = weapon.Master.IsPlayer ?
            LayersManager.Player :
            LayersManager.EnemyProjectile;

        // Set the spawned object active.
        spawned.gameObject.SetActive(true);

        // Actually fire the spawned thing.
        spawned.firedBy = weapon;
        spawned.FireInternal();
        return spawned;
    }

    /// <summary>
    /// Actually does the firing.
    /// </summary>
    protected abstract void FireInternal();
    #endregion
}