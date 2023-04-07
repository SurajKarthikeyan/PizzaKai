using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Shooting
{
    public bool canAlt = true;
    public float timeTillAlt;
    public float timestamp;
    
    //public Rigidbody2D rb;
    public float altTime;
    public float startDashTime;
    public Vector3 direction;
    public float altForce;

    public AudioSource GunAltSound;
    public AudioClip GunAlt;

    

    private float pushPower = 4f;


    // Start is called before the first frame update
    override public void Start()
    {
        
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {

        if(!UIScript.isPaused && !UIScript.isDying)
        {
            base.Update();
            if (!canAlt)
            {
                timestamp += Time.deltaTime;
                if (timestamp > timeTillAlt)
                {
                    canAlt = true;
                    timestamp = 0;
                }
            }

            if (Input.GetMouseButton(1) && canAlt == true)
            {
                GunAltSound.Play();
                canAlt = false;
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
