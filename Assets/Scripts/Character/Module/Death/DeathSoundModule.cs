using UnityEngine;

/// <summary>
/// Implements some basic death actions. If you want something more complicated,
/// please make a new class.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class DeathSoundModule : DeathModule
{
    [SerializeField]
    private AudioClip deathSound;

    protected override void OnDeath()
    {
        if (deathSound)
        {
            Master.PlayClip(deathSound);
        }
    }
}