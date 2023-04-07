using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Vector3 bulletTransform;
    public bool canFire;
    public bool reloading = false;
    public float timer;
    public float timeBetweenFiring;
    public float ammoCountCurrent;
    public float ammoCountMax;
    public float reloadTimer;

    //needed for screen shake
    public CameraFollow cameraScript;

    //I put this here so I could access it in the Flip
    public float rotZ;

    public Animator AnimGunRight;
    public Animator AnimGunLeft;
    public Animator AnimPlayer;
    public PlayerMovement player;

    //Sound
    public AudioSource GunFire;
    public AudioClip GunShoot;
    public AudioSource GunReloading;
    public AudioClip GunReload;

    //Pause and flipping
    public UICode UIScript;
    public Flip flip;
    
    // Start is called before the first frame update
    virtual public void Start()
    {
        
        ammoCountCurrent = ammoCountMax;
        //finds camera
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    virtual public void Update()
    {
        //if the game is not paused, do everything you need to do. If it is paused, you cannot do stuff
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            //sets vector 3 to mouse position to track where they are aiming
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - transform.position;

            //Helps track rotation
            rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            //Debug.Log(transform.rotation);

            if (reloading)
                return;

            if (!canFire)
            {
                timer += Time.deltaTime;
                if (timer > timeBetweenFiring)
                {
                    canFire = true;
                    timer = 0;

                }
            }

            //activates reload;
            if (ammoCountCurrent <= 0)
            {
                StartCoroutine(Reload());
                GunReloading.Play();
            }

            if (UIScript.shotgun.activeSelf == true)
            {
                if (flip.shotgunLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.shotgunFirepointLeft.transform.position.x, flip.shotgunFirepointLeft.transform.position.y, flip.shotgunFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {
                    
                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.shotgunFirepointRight.transform.position.x, flip.shotgunFirepointRight.transform.position.y, flip.shotgunFirepointRight.transform.position.z);
                }
            }

            if (UIScript.tommy.activeSelf == true)
            {
                if (flip.tommyLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.tommyFirepointLeft.transform.position.x, flip.tommyFirepointLeft.transform.position.y, flip.tommyFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.tommyFirepointRight.transform.position.x, flip.tommyFirepointRight.transform.position.y, flip.tommyFirepointRight.transform.position.z);
                }
            }

            if (UIScript.flamethrower.activeSelf == true)
            {
                if (flip.flameLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.flameFirepointLeft.transform.position.x, flip.flameFirepointLeft.transform.position.y, flip.flameFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    bulletTransform = new Vector3(flip.flameFirepointRight.transform.position.x, flip.flameFirepointRight.transform.position.y, flip.flameFirepointRight.transform.position.z);
                }
            }

            //Click mouse button to fire
            if (Input.GetMouseButton(0) && canFire)
            {
                AnimGunRight.SetBool("Fire", true);
                AnimGunLeft.SetBool("Fire", true);
                AnimPlayer.SetBool("Fire", true);
                canFire = false;
                //Debug.Log("Shot bullet");
                //this causes this screen to shake when you shoot
                StartCoroutine(cameraScript.Shake());
                Instantiate(bullet, bulletTransform, Quaternion.identity);
                GunFire.Play();
                //Debug.Log("Playing Sound");


                //counts down ammo
                ammoCountCurrent = ammoCountCurrent - 1;
            }
            else
            {
                AnimGunRight.SetBool("Fire", false);
                AnimGunLeft.SetBool("Fire", false);
                AnimPlayer.SetBool("Fire", false);
            }

            if (Input.GetKey("r"))
            {
                //Debug.Log("R pressed");
                GunReloading.Play();
                StartCoroutine(Reload());
            }
        }
        

        
    }
    virtual public IEnumerator Reload()
    {
        reloading = true;
        AnimGunRight.SetBool("Reload", true);
        AnimGunLeft.SetBool("Reload", true);
        canFire = false;
        yield return new WaitForSeconds(reloadTimer);
        ammoCountCurrent = ammoCountMax;
        reloading = false;
        AnimGunRight.SetBool("Reload", false);
        AnimGunLeft.SetBool("Reload", false);
        canFire = true;
        
    }
}
