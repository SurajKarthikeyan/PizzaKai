using UnityEngine;

public abstract class DamagingWeaponSpawn : WeaponSpawn
{
    #region Variables
    [Tooltip("Layer mask for this projectile " +
        "(what this can collide/interact with).")]
    public LayerMask collisionMask;

    [Tooltip("Amount of damage to deal. This will be rounded to an int.")]
    [SerializeField]
    protected Range damage = new(14);

    [HideInInspector]
    public ModifierChain damageModifier = new();
    #endregion

    #region Properties
    /// <summary>
    /// The actual damage.
    /// </summary>
    public int ActualDamage => damageModifier.Modify(damage.Evaluate()).Round();
    #endregion
}