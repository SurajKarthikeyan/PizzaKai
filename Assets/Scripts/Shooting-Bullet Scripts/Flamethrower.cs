using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Shooting
{

    public GameObject fireBall;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        //Change to flamethrowerScriptableObject
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            if (!gunScriptableObject.canAlt)
            {
                gunScriptableObject.timestamp += Time.deltaTime;
                if (gunScriptableObject.timestamp > gunScriptableObject.timeTillAlt)
                {
                    gunScriptableObject.canAlt = true;
                    gunScriptableObject.timestamp = 0;
                }
            }
            base.Update();

            if (Input.GetMouseButton(1) && gunScriptableObject.canAlt == true)
            {
                GunAltSound.Play();
                gunScriptableObject.canAlt = false;
                //change to flamethrowerscriptableobject
                Instantiate(fireBall, gunScriptableObject.bulletTransform, Quaternion.identity);
            }
        }
        
    }
}
