using System.Collections;
using UnityEngine;

public class SimpleProjectile : WeaponSpawn
{
    #region Variables
    [Tooltip("Amount of damage to deal. This will be rounded to an int.")]
    public Range damage = new(14);

    [Tooltip("How fast does this projectile travel, in units per second.")]
    public Range speed = new(10);

    [Tooltip("How far does this projectile travel?")]
    public Range range = new(20);

    [Tooltip("Random spread of the projectile, in degrees.")]
    public Range spread = new(-5, 5);

    [Tooltip("For automatic weapons, determines how much more spread is caused " +
        "by holding down the trigger.")]
    public float spreadRampUp = 1.05f;

    // [Tooltip("Gravity applied to the projectile.")]
    // public Vector2 gravity = new(0, -9.81f);

    [Tooltip("How many ricochets does this projectile have?")]
    public int ricochets = 0;

    [Tooltip("Layer mask for this projectile (what this can collide with).")]
    public LayerMask collisionMask;

    private int currentRicochets = 0;

    private float deltaDistance = 0;

    private float maxLifetime;

    private float currentLifetime;
    #endregion

    #region Properties
    /// <summary>
    /// Defines the forward vector.
    /// </summary>
    private Vector3 Forwards => transform.forward;
    #endregion

    protected override void Fire(int burstIndex)
    {
        // Apply initial spread.
        float actualSpread = spread.Select() * (spreadRampUp * burstIndex);
        transform.Rotate(new Vector3(0, 0, actualSpread));

        // Calculate lifetime;
        maxLifetime = range.Select() / speed.Select();
        currentLifetime = 0;

        // Calculate delta distance.
        deltaDistance = speed.Select() * Time.fixedDeltaTime;

        StartCoroutine(Fire_CR());
    }

    /// <summary>
    /// This will be shared across all projectiles.
    /// </summary>
    private static readonly RaycastHit2D[] hits = new RaycastHit2D[16];

    private IEnumerator Fire_CR()
    {
        ContactFilter2D contactFilter = new()
        {
            layerMask = collisionMask,
            useLayerMask = true
        };

        while (enabled && currentLifetime < maxLifetime)
        {
            yield return new WaitForFixedUpdate();

            currentLifetime += Time.fixedDeltaTime;

            // Do the raycast things.
            int totalHits = Physics2D.Raycast(
                transform.position,
                Forwards,
                contactFilter,
                hits,
                deltaDistance
            );

            bool hitThing = false;
            bool needsDestroy = false;

            for (int i = 0; i < totalHits; i++)
            {
                var hit = hits[i];

                if (hit.collider)
                {
                    hitThing = true;
                    var collider = hit.collider;

                    if (collider.gameObject.HasComponent(out Character character))
                    {
                        // Deal damage.
                        character.TakeDamage(Mathf.RoundToInt(damage.Evaluate()));
                    }

                    if (currentRicochets < ricochets)
                    {
                        // Handle ricochets. Position projectile very close to
                        // the hit point, but not quite there.
                        Vector2 diff = hit.point - transform.position.ToVector2();
                        transform.position += diff.ToVector3() * 0.95f;

                        // Make the projectile go in the collision normal
                        // direction.
                        transform.forward = Vector3.Reflect(
                            transform.forward,
                            hit.normal.ToVector3()
                        );
                    }
                    else
                    {
                        // No more ricochets.
                        needsDestroy = true;
                        break;
                    }
                }
            }

            if (needsDestroy)
            {
                // Break from loop.
                break;
            }

            if (!hitThing)
            {
                // Hit nothing. Continue on.
                transform.position += Forwards * deltaDistance;
            }
        }

        // Handle any destruction thingies here.
    }
}