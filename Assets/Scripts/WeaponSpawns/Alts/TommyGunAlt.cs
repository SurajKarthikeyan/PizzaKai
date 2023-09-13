using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The alt weapon spawn for Tommy Guns.
/// 
/// <br/>
/// 
/// Fun fact: The full legal name of Tommy Gun is Thomithan Guneon.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class TommyGunAlt : MaskedWeaponSpawn
{
    [Tooltip("Damage this alt does.")]
    public int damage;

    [Tooltip("Amount of knockback applied.")]
    public float knockback = 1;

    [Tooltip("Radius of effect.")]
    public float radius = 10;

    private static readonly List<Collider2D> hits = new();

    protected override void Fire(int burstIndex)
    {
        ContactFilter2D filter = new()
        {
            useLayerMask = true,
            layerMask = collisionMask
        };

        int hitCount = Physics2D.OverlapCircle(
            transform.position.ToVector2(),
            radius,
            filter,
            hits
        );

        for (int i = 0; i < hitCount; i++)
        {
            var hit = hits[i];

            if (hit.gameObject.HasComponent(out Character character))
            {
                // It's fine to use GetComponent (which HasComponent calls)
                // here, as we aren't calling it every frame.
                character.TakeDamage(
                    damage,
                    transform.position.InverseSquare(character.transform.position)
                );
            }
        }
    }
}