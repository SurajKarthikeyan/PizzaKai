using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunWeapon : WeaponModule
{

    private float pushPower = 10f;

    public Rigidbody2D player;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.ShotgunName;
    }


    override public void AltFire()
    {
        weaponAction = "Alt";
        base.AltFire();
        PushPlayer();
                
    }

    private void PushPlayer()
    {
        Vector3 dir = Vector3.Normalize(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        player.velocity = dir * pushPower;
    }


}
