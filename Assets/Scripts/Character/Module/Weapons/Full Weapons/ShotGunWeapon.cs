using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunWeapon : WeaponModule
{

    private float pushPower = 16f;

    public Rigidbody2D player;

    public float altTime;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }


    override public void AltFire()
    {
        base.AltFire();
        Debug.Log("ShotGunAlt");

        StartCoroutine(PushPlayer());

        //Play the proper sound for the gun alt, handled by singleton audio dict
        //Have a boolean and respective logic saying whether or not you can alt, set bool to false here   

    }

    public IEnumerator PushPlayer()
    {
        Vector3 dir = Vector3.Normalize(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));

        player.velocity = dir * pushPower;
        yield return new WaitForSeconds(altTime);
    }


}
