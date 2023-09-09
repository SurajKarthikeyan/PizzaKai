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
        return;
    }

    // Handles taking damage if the generator is vulnerable
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isVulnerable)   // checks if vulnerability is allowed
        {
            base.OnTriggerEnter2D(collision);
        }
    }
    #endregion

    #region PrivateMethods
    // Function to start set the generator to be vulnerable
    public void SetVulnerability()
    {
        StartCoroutine(VulnerabilityWait());
    }

    //Enumerator to set a length of time that it is vulnerable for
    IEnumerator VulnerabilityWait()
    {
        isVulnerable = true;
        yield return new WaitForSeconds(vulnerableTime);
        isVulnerable = false;
    }
    #endregion
    
}
