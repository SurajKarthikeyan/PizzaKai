using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    #region Variables

    public GameObject leverArm; // This is simply a test variable. It might be the case wherein we have a different way to showcase that the lever was activated
    

    #endregion

    #region UnityMethods

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            leverArm.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    #endregion
}
