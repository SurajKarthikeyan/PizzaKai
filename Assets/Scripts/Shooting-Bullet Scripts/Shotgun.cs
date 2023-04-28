using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Shooting
{
    
    
    //public Rigidbody2D rb;
    public float altTime;
    public float startDashTime;
    public Vector3 direction;
    public float altForce;


    private float pushPower = 15f;
    
    

    // Start is called before the first frame update
    override public void Start()
    {
        
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        //Change to shotgunScriptableObject
        if(!UIScript.isPaused && !UIScript.isDying)
        {
            base.Update();
            if (!gunScriptableObject.canAlt)
            {
                gunScriptableObject.timestamp += Time.deltaTime;
                if (gunScriptableObject.timestamp > gunScriptableObject.timeTillAlt)
                {
                    gunScriptableObject.canAlt = true;
                    gunScriptableObject.timestamp = 0;
                }
            }

            if (Input.GetMouseButton(1) && gunScriptableObject.canAlt == true)
            {
                GunAltSound.Play();
                gunScriptableObject.canAlt = false;
                player.isAlting = true;
                StartCoroutine(cameraScript.Shake());
                StartCoroutine(altFire());
                StartCoroutine(player.altCool());
            }
        }

    }

    public IEnumerator altFire()
    {
        Vector3 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        player.rigid.AddForce(dir * pushPower, ForceMode2D.Force);

        // as alternative:
        player.rigid.velocity = dir * pushPower;
        yield return new WaitForSeconds(altTime);
    }
}
