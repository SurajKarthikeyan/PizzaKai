using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generator : EnemyBasic
{
    #region  Variables
    [Tooltip("Boolean variable to see if the generator is vulnerable")]
    [SerializeField] private bool isVulnerable;
    [Tooltip("Int variable to set the duration of the vulnerability in a coroutine")]
    [SerializeField] private int vulnerableTime;
    [SerializeField] private AudioSource generatorSource;
    [SerializeField] private ShakyGenerator shakyGen;
    [SerializeField] private Animator generatorAnimator;
    private static readonly int IsVulnerable = Animator.StringToHash("isVulnerable");

    #endregion

    #region  UnityMethods
    public override void Start()
    {
        base.Start();
        isVulnerable = false;
    }

    // Overrided to do nothing to avoid console errors with pathfinding (generator doesn't need pathfinding)
    public override void Update()
    {
        if (currentHP <= 0)
        {
            deathState = true;
            OnDeath();
        }
    }

    // Handles taking damage if the generator is vulnerable
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isVulnerable)   // checks if vulnerability is allowed
        {
            AudioDictionary.aDict.PlayAudioClipRemote("generatorDamageGood", generatorSource);
            base.OnTriggerEnter2D(collision);
        }
        else
        {
            AudioDictionary.aDict.PlayAudioClipRemote("generatorDamageBad", generatorSource);
        }
    }
    #endregion

    #region PrivateMethods
    // Function to start set the generator to be vulnerable
    public void SetVulnerability()
    {
        generatorAnimator.SetBool(IsVulnerable, true);
        StartCoroutine(VulnerabilityWait());
        shakyGen.startShaking(vulnerableTime);
        // Works, need to change sprite now
    }

    //Enumerator to set a length of time that it is vulnerable for
    IEnumerator VulnerabilityWait()
    {
        isVulnerable = true;
        yield return new WaitForSeconds(vulnerableTime);
        isVulnerable = false;
        generatorAnimator.SetBool(IsVulnerable, false);

    }

    private void OnDeath()
    {
        // Play Death Animation
        // Instantiate alternative sprite
        AudioDictionary.aDict.PlayAudioClipRemote("generatorDead", generatorSource);
        Destroy(this.gameObject);
        
    }
    #endregion
    
}
