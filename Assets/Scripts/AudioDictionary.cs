using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AudioDictionary : MonoBehaviour
{
    #region Vars
    // An enuemrator that is there to differentiate states of which source is being used
    public enum Source 
    {
        Test,
        Player,
        Bread,
        Chicken,
        Molotov,
        Deepdish
    };
    public UnityDictionary<string, AudioClip> audioDict;
    public Source source;
    [Header("Audio Sources")]
    [Tooltip("Audio Source for testing")]
    [SerializeField] public AudioSource testSource;
    [Tooltip("Audio Source for the player")]
    [SerializeField] public AudioSource playerSource;
    [Tooltip("Audio Source for the bread")]
    [SerializeField] public AudioSource breadSource;
    [Tooltip("Audio Source for the chicken")]
    [SerializeField] public AudioSource chickenSource;
    [Tooltip("Audio Source for the molotov")]
    [SerializeField] public AudioSource molotovSource;
    [Tooltip("Audio Source for the molotov")]
    [SerializeField] public AudioSource deepDishSource;
    public static AudioDictionary aDict;
    #endregion

    #region UnityMethods
    private void Start()
    {
        if(aDict == null)
        {
            aDict = this;
        }
        else
        {
            Debug.LogError("You done goofed and made two audio Dictionaries in the scene");  
        }
    }
    #endregion
    #region PrivateMethods
    // Gets the key along with which source to use and then sets the clip to that source and plays it
    public void PlayAudioClip(string clipKey, Source tempSource)
    {
        AudioClip tempClip = audioDict[clipKey];  // find temporary clip
        
        switch(tempSource) 
        {
            case Source.Test:
                testSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                testSource.Play();
                break;
            case Source.Player:
                playerSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                playerSource.Play();
                break;
            case Source.Bread:
                breadSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                breadSource.Play();
                break;
            case Source.Chicken:
                chickenSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                chickenSource.Play();
                break;
            case Source.Molotov:
                molotovSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                molotovSource.Play();
                break;
            case Source.Deepdish:
                deepDishSource.clip = tempClip; // assign and play specific clip to specific source according to parameter
                deepDishSource.Play();
                break;
        }
    }
    
    /// <summary>
    /// Non Local version of Play Audio clip. Instead of using a audio clip attached this this GO, it uses one passed in
    /// by reference by another GO
    /// </summary>
    /// <param name="clipKey"></param>
    /// <param name="remoteSource"></param>
    public void PlayAudioClipRemote(string clipKey, AudioSource remoteSource)
    {
        remoteSource.clip = audioDict[clipKey];
        remoteSource.Play();
    }
    #endregion
}