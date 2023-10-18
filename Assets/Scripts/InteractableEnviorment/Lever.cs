using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject leverArm; // This is simply a test variable. It might be the case wherein we have a different way to showcase that the lever was activated
    [Tooltip("Time wherein the lever is unable to be shot again and is deactivated")]   // goes magenta 
    [SerializeField] private float disabledTime;
    [Tooltip("Time wherein the lever is unable to be shot again but is activated")] // stays red
    [SerializeField] private float enabledTime;
    private bool canBeShot = true;
    

    #endregion

    #region PrivateMethods

    public void LeverArmActivate()
    {
        if (canBeShot)
        {
            AudioDictionary.aDict.PlayAudioClip("leverSwitch", AudioDictionary.Source.Lever);
            StartCoroutine(EnabledTime());
        }
    }

    IEnumerator EnabledTime()
    {
        //Active, unable to be shot again
        canBeShot = false;
        leverArm.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(enabledTime);
        StartCoroutine(DisabledTime());
    }

    IEnumerator DisabledTime()
    {
        //Not active, unable to be shot again
        leverArm.GetComponent<SpriteRenderer>().color = Color.magenta;
        //------------------Play different SFX here???????--------------------//
        yield return new WaitForSeconds(disabledTime);
        leverArm.GetComponent<SpriteRenderer>().color = Color.white;
        canBeShot = true;
        //------------------Play Enable SFX here???????--------------------//
    }

    #endregion
}
