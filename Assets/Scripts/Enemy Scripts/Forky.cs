using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Responsible for this file: Zane O'Dell
/// 
/// This class will handle the logic for Forky, including animations and instantiating boxes
/// 
/// <br>
/// 
/// Authors: Zane O'Dell
/// </summary>
public class Forky : MonoBehaviour
{

    public enum BossState
    {
        FullHealth, 
        OneHit,
        TwoHits,
        Death
    }

    BossState state;

    public AudioSource forkyAudioSource;

    public Animator forkyAnimator;

    //Reference to the oven script to use for health

    // Start is called before the first frame update
    void Start()
    {
        BossState state = BossState.FullHealth;
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.FullHealth:
                //Instantiate boxes at appropriate height and interval (interval function below)
                break;
            case BossState.OneHit:
                break;
            case BossState.TwoHits:
                break;
            case BossState.Death:
                //Play Death Animation once and destroy object
                break;
        }

    }

    void NextPhase()
    {
        state++;
    }
}
