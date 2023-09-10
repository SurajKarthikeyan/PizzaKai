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

        public readonly bool IsPlayer => tag == "Player";

        public FiredByIdentifier(Character character)
        {
            layer = new(character.gameObject.layer);
            tag = character.gameObject.tag;
        }
    }
    #endregion

    #region Variables
    /// <summary>
    /// What fired this spawn thingy?
    /// </summary>
    public FiredByIdentifier firedBy;
    #endregion

    #region Methods
    /// <summary>
    /// Fires this weapon spawn thing.
    /// </summary>
    /// <param name="weapon">The weapon that spawned this.</param>
    /// <param name="burstIndex">Applies only to automatic weapons (non-auto
    /// weapons have this set to 1). This signifies that this is the nth bullet
    /// of the current burst, starting from 1.</param>
    public void Fire(WeaponModule weapon, int burstIndex)
    {
        firedBy = new(weapon.Master);
        Fire(burstIndex);
    }

    protected abstract void Fire(int burstIndex);
    #endregion
}