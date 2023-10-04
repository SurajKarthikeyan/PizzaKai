using UnityEngine;

public abstract class MaskedWeaponSpawn : WeaponSpawn
{
    #region Variables
    [Tooltip("Layer mask for this projectile " +
        "(what this can collide/interact with).")]
    public LayerMask collisionMask;
    #endregion
}