using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RavioliPathing : Character
{
    #region Vars

    [SerializeField] private AudioSource ravioliScream;
    [SerializeField] private AudioSource ravioliExplode;
    [Tooltip("Set to 0.65 seconds because that was enough delay to play audio source and destroy GO without looking bad. Do not change unless smart")]
    [SerializeField] private float destroyDelay;
    [Tooltip("Distance to the player at which to explode at")]
    [SerializeField] private float distanceCheck;
    private GameObject playerRef;
    #endregion

    #region UnityMethods

    private void Start()
    {
        playerRef = GameObject.Find("Player");  // im sorry but im also lazy and low key this doesn't affect performance so idc
    }

    public override void Update()
    {
        base.Update();
        if (DistanceToPlayer() <= distanceCheck)    // if within the bounds to explode, explode
        {
            Die();
        }
    }
    

    #endregion
    
    #region PrivateMethods

    private float DistanceToPlayer()
    {
        return(Mathf.Abs(Vector2.Distance(this.transform.position, playerRef.transform.position)));
        
    }
    
    public override void Die()
    {
        //visual syncing - just disabled the SR so it looks like the sprite is gone to make it easier
        // This all happens in a span of under a second anyways so I'm not really worried
        this.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(WaitForSound(destroyDelay));
    }

    IEnumerator WaitForSound(float delay)
    {
        // Play audio, wait then destory
        AudioDictionary.aDict.PlayAudioClipRemote("ravioliScream", ravioliScream);
        AudioDictionary.aDict.PlayAudioClipRemote("ravioliExplode", ravioliExplode);
        Instantiate(ExplosionManager.explosionManager.SelectExplosionRandom(this.gameObject.transform.position, 0f));
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    #endregion
}
