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
    public Source source;
    //Please ensure the length of these two lists are identical. It is a must to not have an index out of bounds error
    [Tooltip("Make the name of the strings here the same as the animation they'd be played in IF there is a relavent animation")]
    [ReorderableList]
    [SerializeField] private List<string> aClipKeys;    // keys in dict
    [ReorderableList]
    [SerializeField] private List<AudioClip> aClipValues;   // vals in dict
    [System.NonSerialized] public Dictionary<string, AudioClip> audioDictionary;


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

        audioDictionary = new Dictionary<string, AudioClip>();

        if(aDict == null)
        {
            aDict = this;
        }
        else
        {
            Debug.LogError("You done goofed and made two audio Dictionaries in the scene");  
        }

        MakeDictionary();
    }
    #endregion

    #region PrivateMethods
    public void MakeDictionary()
    {
        for (int i = 0; i < aClipKeys.Count; i++)
        {
            audioDictionary[aClipKeys[i]] = aClipValues[i]; // assigns the indexed key to the indexed value. Order technically matters 
            // but thats handled in the inspector anyways so who cares, I just like constant look up time so hash it out with yourself I guess
            //
            // get it?
        }
    }

    // Gets the key along with which source to use and then sets the clip to that source and plays it
    public void PlayAudioClip(string clipKey, Source tempSource)
    {
        AudioClip tempClip = audioDictionary[clipKey];  // find temporary clip
        
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
    
    #endregion
}
