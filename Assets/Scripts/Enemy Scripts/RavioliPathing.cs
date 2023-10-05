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
    [FormerlySerializedAs("destoryDelay")]
    [Tooltip("Set to 0.4 seconds because that was enough delay to play audio source and destroy GO without looking bad. Do not change unless smart")]
    [SerializeField] private float destroyDelay;
    #endregion

    #region PrivateMethods

    public override void Die()
    {
        StartCoroutine(WaitForSound(destroyDelay));
    }

    IEnumerator WaitForSound(float delay)
    {
        AudioDictionary.aDict.PlayAudioClipRemote("ravioliScream", ravioliScream);
        //yield return new WaitWhile(() => ravioliScream.isPlaying);
        AudioDictionary.aDict.PlayAudioClipRemote("ravioliExplode", ravioliExplode);
        Instantiate(ExplosionManager.explosionManager.SelectExplosionRandom(this.gameObject.transform.position, 0f));
        //yield return new WaitWhile(() => ravioliExplode.isPlaying);
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    #endregion
}
