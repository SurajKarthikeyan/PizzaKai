using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerWeapon : WeaponModule
{
    public GameObject bigBullet;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.FlamethrowerName;
    }

    override public void AltFire()
    {
        
        base.AltFire();
        Debug.Log("Flamethrower Alt fire");
        Instantiate(bigBullet, firePoint.position, Quaternion.identity);
        //Play the proper sound for the gun alt, handled by singleton audio dict
        //AudioDictionary.aDict.PlayAudioClip("shotgunAlt", AudioDictionary.Source.Player);
    }
}
