using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Shooting
{

    private Camera mainCamera;

    private float mainFOV;
    private float sniperFOV = 10f;

    private enum AimState
    {
        regularFOV, 
        ADSFOV
    };


    AimState aimState;
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainFOV = mainCamera.orthographicSize;
        aimState = AimState.regularFOV;
    }

    // Update is called once per frame
    override public void Update()
    {

        if (!UI.isPaused && !UI.isDying)
        {

            
            //if (aimState == AimState.regularFOV)
            //{
            //    gunScriptableObject.timestamp += Time.deltaTime;
            //    if (gunScriptableObject.timestamp > gunScriptableObject.timeTillAlt)
            //    {
            //        gunScriptableObject.canAlt = false;
            //        gunScriptableObject.timestamp = 0;
            //    }
            //}

            base.Update();

            if (Input.GetMouseButton(1))
            {
                GunAltSound.Play();
                StartCoroutine(SwitchFOV());
            }

        }

    }
    
    private IEnumerator SwitchFOV()
    {
        //Needs UI indicator where the alt fire is for most guns for the aiming mode. 
        if (aimState == AimState.regularFOV)
        {
            
            mainCamera.orthographicSize = sniperFOV;
            Cursor.visible = true;
            yield return new WaitForSeconds(2);
            aimState = AimState.ADSFOV;
            Debug.Log("Sniper Aim");
        }
        else
        {
            
            mainCamera.orthographicSize = mainFOV;
            Cursor.visible = false;
            yield return new WaitForSeconds(2);
            aimState = AimState.regularFOV;
            Debug.Log("Exiting Aim");
        }
        

    }
    
}
