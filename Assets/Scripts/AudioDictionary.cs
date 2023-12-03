using UnityEngine;

/// <summary>
/// Class to store and handle playing of Audio in the game
/// Two main methods; PlayAudioClip & PlayAudioClipRemote
/// One handles it with local(to this GO) Audio Sources
/// Remote handles it with a passed in Source via a parameter
///
///
///
/// Authors: (Main) Suraj Karthikeyan (2023)
/// </summary>
public class AudioDictionary : MonoBehaviour
{
    #region Vars
    // An enuemrator that is there to differentiate states of which source is being used
    public enum Source 
    {
        Test,
        Gun,
        Player,
        Generator,
        Lever,
    };
    public UnityDictionary<string, AudioClip> audioDict;
    public Source source;
    [Header("Audio Sources")]
    [Tooltip("Audio Source for testing")]
    [SerializeField] public AudioSource testSource;
    [Tooltip("Audio Source for shooting weapon [Generalized]")] 
    [SerializeField] public AudioSource gunSource;
    [Tooltip("Audio Source for general player SFX")] 
    [SerializeField] public AudioSource playerSource;
    [Tooltip("Audio Source for general player SFX")] 
    [SerializeField] public AudioSource generatorSource;
    [Tooltip("Audio Source for lever SFX")] 
    [SerializeField] public AudioSource leverSource;
    public static AudioDictionary aDict;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (aDict == null)
        {
            aDict = this;
        }
        else
        {
            Debug.LogError("You done goofed and made two audio Dictionaries in the scene");
            Destroy(gameObject);
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
            case Source.Gun:
                gunSource.clip = tempClip;
                gunSource.Play();
                break;
            case Source.Player:
                playerSource.clip = tempClip;
                playerSource.Play();
                break;
            case Source.Generator:
                generatorSource.clip = tempClip;
                generatorSource.Play();
                break;
            case Source.Lever:
                leverSource.clip = tempClip;
                leverSource.Play();
                break;
        }
    }
    
    /// <summary>
    /// Non Local version of Play Audio clip. Instead of using a audio source attached this this GO, it uses one passed in
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