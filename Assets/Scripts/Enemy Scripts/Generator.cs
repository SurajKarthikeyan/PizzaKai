using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Generator : EnemyBasic
{
    #region  Variables
    [Tooltip("Boolean variable to see if the generator is vulnerable")]
    [SerializeField] private bool isVulnerable;
    [Tooltip("Int variable to set the duration of the vulnerability in a coroutine")]
    [SerializeField] public int vulnerableTime;
    [SerializeField] private AudioSource generatorSource;
    [SerializeField] private ShakyGenerator shakyGen;
    [SerializeField] private Animator generatorAnimator;
    [SerializeField] private GameObject explosionGO;
    
    [Tooltip("Manually transfer the keys from AudioDict to here. Its shit but IDK how to do it as of now")] // Note, you were too drunk
    [SerializeField] private List<string> explosionKeyList;
    private static readonly int IsVulnerable = Animator.StringToHash("isVulnerable");
    private Forky forky;

    private GameObject reticleGO;


    #endregion

    #region  UnityMethods
    public override void Start()
    {
        base.Start();
        isVulnerable = false;
        forky = GameObject.Find("Forky").GetComponent<Forky>();
        reticleGO = transform.GetChild(1).gameObject;
    }

    // Overrided to do nothing to avoid console errors with pathfinding (generator doesn't need pathfinding)
    public override void Update()
    {
        if (currentHP <= 0)
        {
            deathState = true;
            OnDeath();
        }

        if (isVulnerable)
        {
            reticleGO.transform.Rotate(new Vector3(0, 0, -50 * Time.deltaTime));
        }
        else
        {
            reticleGO.transform.rotation = Quaternion.identity;
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
        reticleGO.SetActive(true);
        yield return new WaitForSeconds(vulnerableTime);
        isVulnerable = false;
        reticleGO.SetActive(false);
        generatorAnimator.SetBool(IsVulnerable, false);
    }

    private void OnDeath()
    {
        // Play Death Animation
        // Instantiate alternative sprite
        string key = explosionKeyList[Random.Range(0, explosionKeyList.Count)];
        Debug.Log(key);
        AudioDictionary.aDict.PlayAudioClip(key, AudioDictionary.Source.Generator);
        // how do I handle randomization without having to re-code this?
        ExplosionManager.Instance.SelectExplosionRandom(this.gameObject.transform.position, 180f);
        forky.NextPhase();
        Destroy(gameObject);
        
    }
    
    public override void TakeDamage(int damage)
    {
        if (isVulnerable)   // checks if vulnerability is allowed
        {
            AudioDictionary.aDict.PlayAudioClipRemote("generatorDamageGood", generatorSource);
            base.TakeDamage(damage);
        }
        else
        {
            AudioDictionary.aDict.PlayAudioClipRemote("generatorDamageBad", generatorSource);
        }
    }
    #endregion

}
