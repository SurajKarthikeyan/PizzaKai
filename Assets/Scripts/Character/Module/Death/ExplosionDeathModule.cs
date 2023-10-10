using UnityEngine;

/// <summary>
/// Spawns an explosion on death.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class ExplosionDeathModule : DeathModule
{
    protected override void OnDeath()
    {
        ExplosionManager.Instance.SelectExplosionRandom(transform.position, 0);
        Master.r2d.bodyType = RigidbodyType2D.Static;
        Master.c2d.enabled = false;
        Master.sr.enabled = false;
    }
}