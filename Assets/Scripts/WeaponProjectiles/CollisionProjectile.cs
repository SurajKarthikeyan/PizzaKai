using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a projectiles that uses either a trigger or a normal collider to
/// deal damage.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class CollisionProjectile : DamagingWeaponSpawn
{
    #region Enums
    /// <summary>
    /// Describes the options of the projectile.
    /// </summary>
    [System.Flags]
    public enum Flags
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,
        /// <summary>
        /// Kills the projectile when colliding with something. Requires 
        /// <see cref="attachedCharacter"/>. Called first.
        /// </summary>
        KillOnCollide = 1,
        /// <summary>
        /// Invokes a customizable Unity Event.
        /// </summary>
        InvokeEvent = 2,
        /// <summary>
        /// Destroys the gameobject on collision. Called after <see
        /// cref="KillOnCollide"/>.
        /// </summary>
        DestroyOnCollide = 4,
    }
    #endregion

    #region Variables
    [InfoBox("The flags for the collision projectile. Number is order in " +
        "which they are called.\n" +
    "None: No flags.\n" +
    "[1] KillOnCollide: Kills the projectile when colliding with something.\n" +
    "[2] InvokeEvent: Invokes a customizable Unity Event.\n" +
    "[3] DestroyOnCollide: Destroys the gameobject on collision."
    )]
    public Flags flags;

    [Tooltip("The character associated with this projectile.")]
    [ShowIf(nameof(AttachedCharacter_ShowIf))]
    public Character attachedCharacter;

    [ShowIf(nameof(Event_ShowIf))]
    public UnityEvent<CollisionProjectile> projectileEvent;

    public float force = 15;
    #endregion

    #region Editor Functions
    /// <summary>
    /// Shows the <see cref="attachedCharacter"/> field if this returns true.
    /// </summary>
    /// <returns></returns>
    private bool AttachedCharacter_ShowIf()
    {
        return flags.AllFlagsSet(Flags.KillOnCollide);
    }

    /// <summary>
    /// Shows the <see cref="projectileEvent"/> and <see cref="generalEvent"/>
    /// if this returns true.
    /// </summary>
    /// <returns></returns>
    private bool Event_ShowIf()
    {
        return flags.AllFlagsSet(
            Flags.InvokeEvent
        );
    }
    #endregion

    #region Weapon Spawn Implementation
    protected override void FireInternal()
    {
        // Yeet.
        if (gameObject.HasComponent(out Rigidbody2D r2d))
        {
            r2d.AddRelativeForce(new Vector2(force, 0), ForceMode2D.Impulse);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        TestCollision(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TestCollision(other.gameObject);
    }

    private void TestCollision(GameObject collideWith)
    {
        if (CollisionMask.ContainsLayer(collideWith.layer))
        {
            if (flags.HasFlag(Flags.KillOnCollide))
            {
                attachedCharacter.Die();
            }
            if (flags.HasFlag(Flags.InvokeEvent))
            {
                projectileEvent.Invoke(this);
            }
            if (flags.HasFlag(Flags.DestroyOnCollide))
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion
}