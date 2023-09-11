using UnityEngine;

/// <summary>
/// Implements some basic death actions. If you want something more complicated,
/// please make a new class.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class GeneralDeathModule : Module
{
    [SerializeField]
    private AudioClip deathSound;

    protected override void OnLinked()
    {
        base.OnLinked();

        Master.onCharacterDeathEvent.AddListener(PlayDeathSound);
    }

    private void PlayDeathSound()
    {
        if (deathSound)
        {
            Master.PlayClip(deathSound);
        }
    }
}