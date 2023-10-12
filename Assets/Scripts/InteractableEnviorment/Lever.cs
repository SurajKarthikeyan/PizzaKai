using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    #region Variables

    public GameObject leverArm; // This is simply a test variable. It might be the case wherein we have a different way to showcase that the lever was activated
    

    #endregion

    #region PrivateMethods

    public void LeverArmActivate()
    {
        AudioDictionary.aDict.PlayAudioClip("leverSwitch", AudioDictionary.Source.Lever);
        leverArm.GetComponent<SpriteRenderer>().color = Color.red;
    }

    #endregion
}
