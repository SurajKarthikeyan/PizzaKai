using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This seems unessesary as a script due to its simplicity, its just a personal preference
/// If it causes a lot of probelms, revert the call back to the ForkyOven script but it should be that bad
/// </summary>
public class Alarm : MonoBehaviour
{
    #region Vars
    [SerializeField] private AudioSource alarmSource;
    [SerializeField] private SpriteRenderer sRend;
    #endregion

    private void Update()
    {
        if (alarmSource.isPlaying)
        {
            sRend.color = Color.red;
        }
        else
        {
            sRend.color = Color.white;
        }
    }

    public void AlarmSystem()
    {
        AudioDictionary.aDict.PlayAudioClipRemote("alarmWarehouse", alarmSource);
    }
}
