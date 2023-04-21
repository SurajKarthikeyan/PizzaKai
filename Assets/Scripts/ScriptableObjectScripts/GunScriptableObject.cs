using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GunScriptableObject", menuName = "ScriptableObjects/GunScriptableObject")]
public class GunScriptableObject : ScriptableObject {

    
    public Vector3 mousePos;
    public GameObject bullet;
    public Vector3 bulletTransform;
    public bool canFire;
    public bool reloading = false;
    public float timer;
    public float timeBetweenFiring;
    public float ammoCountCurrent;
    public float ammoCountMax;
    public float reloadTimer;
    public float damageMultiplier = 1;

    

    //I put this here so I could access it in the Flip
    public float rotZ;


    public bool canAlt = true;
    public float timeTillAlt;
    public float timestamp;

    public AudioClip GunShoot;

    public AudioClip GunReload;

    public AudioClip GunAlt;

}
