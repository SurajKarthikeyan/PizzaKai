using System.Collections;
using UnityEngine;

public class SimpleProjectile : WeaponSpawn
{
    #region Variables
    [Tooltip("Number of projectiles to spawn.")]
    public int count = 1;

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

    private int currentRicochets = 0;

    private float deltaDistance = 0;

    private float maxLifetime;

    private float currentLifetime;
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

    private IEnumerator Fire_CR()
    {
        while (enabled && currentLifetime < maxLifetime)
        {
            yield return new WaitForFixedUpdate();

            currentLifetime += Time.fixedDeltaTime;

            // Do the raycast things.
            
        }
    }
}