using NaughtyAttributes;
using UnityEngine;

public abstract class DamagingWeaponSpawn : WeaponSpawn
{
    #region Variables
    [Tooltip("Whether or not to override the default collision mask provided " +
        "by LayersManager.")]
    [SerializeField]
    private bool overrideMask = false;

    [Tooltip("Layer mask for this projectile " +
        "(what this can collide/interact with).")]
    [ShowIf(nameof(overrideMask))]
    [SerializeField]
    private LayerMask collisionMask;

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

    /// <summary>
    /// Layer mask for this projectile (what this can collide/interact with).
    /// </summary>
    protected LayerMask CollisionMask
    {
        get
        {
            if (overrideMask)
                return collisionMask;

            return gameObject.layer == LayersManager.Player
                ? LayersManager.Instance.playerBulletsCollision
                : LayersManager.Instance.enemyBulletsCollision;
        }
    }
    #endregion
}