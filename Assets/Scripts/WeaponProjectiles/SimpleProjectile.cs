using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleProjectile : MaskedWeaponSpawn
{
    #region Variables
    [Tooltip("Amount of damage to deal. This will be rounded to an int.")]
    public Range damage = new(14);

    [Tooltip("Amount of knockback applied.")]
    public Range knockback = new(1);

    [Tooltip("How fast does this projectile travel, in units per second.")]
    public Range speed = new(10);

    [Tooltip("How far does this projectile travel?")]
    public Range range = new(20);

    [Tooltip("Base random spread of the projectile, in degrees.")]
    public Range spread = new(-5, 5);

    [Tooltip("Determines how fast the spread increases. Higher values = " +
        "faster spread increase.")]
    public float spreadRampUp = 1.05f;

    [Tooltip("The maximum multiplier for the spread.")]
    public float spreadMaxMult = 2.5f;

    // [Tooltip("Gravity applied to the projectile.")]
    // public Vector2 gravity = new(0, -9.81f);

    [Tooltip("How many ricochets does this projectile have?")]
    public int ricochets = 0;

    [Tooltip("[Optional] What to spawn on death?")]
    public GameObject spawnOnDeath;

    private int currentRicochets = 0;

    private float deltaDistance = 0;

    private Duration lifetime;

    private ContactFilter2D contactFilter;
    #endregion

    #region Properties
    /// <summary>
    /// Defines the "forward" vector.
    /// </summary>
    private Vector3 Forwards => transform.right;
    #endregion

    protected override void FireInternal()
    {
        float recoilRatio = 1;

        if (firedBy.autofire)
        {
            // Apply initial spread.
            int burstCnt = (firedBy.autofire && firedBy.firstShotAccurate) ?
                firedBy.BurstCount - 1 :
                firedBy.BurstCount;
    
            recoilRatio = burstCnt * spreadMaxMult /
                (burstCnt + (1 / spreadRampUp));
        }

        // print(recoilRatio);

        float actualSpread = spread.Evaluate() * recoilRatio;
        transform.Rotate(new Vector3(0, 0, actualSpread));

        // Calculate lifetime;
        lifetime = new(range.Evaluate() / speed.Evaluate());

        // Calculate delta distance.
        deltaDistance = speed.Evaluate() * Time.fixedDeltaTime;

        contactFilter = new()
        {
            layerMask = collisionMask,
            useLayerMask = true
        };
    }

    /// <summary>
    /// This will be shared across all projectiles.
    /// </summary>
    private static readonly RaycastHit2D[] hits = new RaycastHit2D[4];

    private void FixedUpdate()
    {
        if (gameObject.activeInHierarchy && !lifetime.IncrementFixedUpdate(false))
        {
            // Do the raycast things.
            int totalHits = Physics2D.Raycast(
                transform.position,
                Forwards,
                contactFilter,
                hits,
                deltaDistance
            );

            for (int i = 0; i < totalHits; i++)
            {
                var hit = hits[i];

                if (hit.collider)
                {
                    bool hitCharacter = false;
                    var collider = hit.collider;

                    if (collider.gameObject.HasComponent(out Character character))
                    {
                        // Deal damage.
                        character.TakeDamage(
                            Mathf.RoundToInt(damage.Evaluate()),
                            Forwards * knockback.Evaluate()
                        );
                        hitCharacter = true;
                    }
                    else if (collider.gameObject.HasComponent(out EnemyBasic enemyBasic))
                    {
                        enemyBasic.TakeDamage(Mathf.RoundToInt(damage.Evaluate()));
                    }
                    else if (collider.gameObject.HasComponent<BurningScript>() && CompareTag("PlayerFireBullet"))
                    {
                        collider.gameObject.GetComponent<BurningScript>().BurnBox();
                    }

                    if (currentRicochets < ricochets)
                    {
                        // Handle ricochets. Position projectile very close to
                        // the hit point, but not quite there.
                        Vector2 diff = hit.point - transform.position.ToVector2();
                        transform.position += diff.ToVector3() * 0.95f;

                        // Make the projectile go in the collision normal
                        // direction.
                        transform.right = Vector3.Reflect(
                            Forwards,
                            hit.normal.ToVector3()
                        );
                    }
                    else
                    {
                        // No more ricochets.
                        DestroyProjectile();
                        return;
                    }

                    if (hitCharacter)
                    {
                        // Hit a character. Break from loop. 
                        break;
                    }
                }
            }

            // Continue on.
            transform.position += Forwards * deltaDistance;
        }
        else
        {
            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        // Handle any destruction thingies here.
        if (spawnOnDeath)
        {
            var inst = Instantiate(spawnOnDeath);
            inst.transform.MatchOther(transform);
            inst.SetActive(true);
        }

        Destroy(gameObject);
    }
}