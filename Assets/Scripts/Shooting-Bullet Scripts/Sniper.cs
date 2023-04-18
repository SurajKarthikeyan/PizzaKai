using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Shooting
{

    private Camera mainCamera;

    private float mainFOV;
    private float sniperFOV = 10f;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainFOV = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    override public void Update()
    {

        if (!UI.isPaused && !UI.isDying)
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

            if (Input.GetMouseButton(1))
            {
                if (gunScriptableObject.canAlt)
                {
                    GunAltSound.Play();
                    mainCamera.fieldOfView = sniperFOV;
                    gunScriptableObject.canAlt = false;
                    Debug.Log("Sniper Aim");

                    
                }
                else
                {
                    Debug.Log("Exiting Aim");
                    mainCamera.fieldOfView = mainFOV;
                    gunScriptableObject.canAlt = true;
                }

            }

        }

    }
    

    
}
