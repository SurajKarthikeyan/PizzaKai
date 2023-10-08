using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Serialization;

public class RavioliPathing : Character
{
    #region Vars

    [SerializeField] private AudioSource ravioliScream;
    [SerializeField] private AudioSource ravioliExplode;
    [Tooltip("Set to 0.65 seconds because that was enough delay to play audio source and destroy GO without looking bad. Do not change unless smart")]
    [SerializeField] private float destroyDelay;
    #endregion

    #region PrivateMethods

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
