using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GunScriptableObject gunScriptableObject;

    private Camera mainCam;

    public Animator AnimGunRight;
    public Animator AnimGunLeft;
    public Animator AnimPlayer;

    // To detect Mouse presses
    private bool heldDown = false;


    //Sound
    public AudioSource GunFire;
    public AudioSource GunReloading;
    public AudioSource GunAltSound;
    

    //needed for screen shake
    public CameraFollow cameraScript;

    public PlayerMovement player;

    //Pause and flipping
    public UICode UIScript;
    public Flip flip;

    public List<SpriteRenderer> visibleEnemies = new List<SpriteRenderer>();

    public UICode UI;


    // Start is called before the first frame update
    virtual public void Start()
    {

        gunScriptableObject.ammoCountCurrent = gunScriptableObject.ammoCountMax;
        //finds camera
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gunScriptableObject.canFire = true;
    }

    // Update is called once per frame
    virtual public void Update()
    {
        //if the game is not paused, do everything you need to do. If it is paused, you cannot do stuff
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            //sets vector 3 to mouse position to track where they are aiming
            gunScriptableObject.mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = gunScriptableObject.mousePos - transform.position;

            //Helps track rotation
            gunScriptableObject.rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, gunScriptableObject.rotZ);

            //Debug.Log(transform.rotation);

            if (gunScriptableObject.reloading)
                return;

            if (!gunScriptableObject.canFire)
            {
                gunScriptableObject.timer += Time.deltaTime;
                if (gunScriptableObject.timer > gunScriptableObject.timeBetweenFiring)
                {
                    gunScriptableObject.canFire = true;
                    gunScriptableObject.timer = 0;

                }
            }

            //activates reload;
            if (gunScriptableObject.ammoCountCurrent <= 0)
            {
                StartCoroutine(Reload());
                GunReloading.Play();
            }

            if (UIScript.shotgun.activeSelf == true)
            {
                if (flip.shotgunLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.shotgunFirepointLeft.transform.position.x, 
                        flip.shotgunFirepointLeft.transform.position.y, 
                        flip.shotgunFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.shotgunFirepointRight.transform.position.x, 
                        flip.shotgunFirepointRight.transform.position.y, 
                        flip.shotgunFirepointRight.transform.position.z);
                }
            }

            if (UIScript.tommy.activeSelf == true)
            {
                if (flip.tommyLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.tommyFirepointLeft.transform.position.x, 
                        flip.tommyFirepointLeft.transform.position.y, 
                        flip.tommyFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.tommyFirepointRight.transform.position.x, 
                        flip.tommyFirepointRight.transform.position.y, 
                        flip.tommyFirepointRight.transform.position.z);
                }
            }

            if (UIScript.flamethrower.activeSelf == true)
            {
                if (flip.flameLeft.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.flameFirepointLeft.transform.position.x, 
                        flip.flameFirepointLeft.transform.position.y, 
                        flip.flameFirepointLeft.transform.position.z);
                }
                if (flip.shotgunRight.activeSelf == true)
                {

                    //make left firepoint the main firepoint
                    gunScriptableObject.bulletTransform = new Vector3(flip.flameFirepointRight.transform.position.x, 
                        flip.flameFirepointRight.transform.position.y, 
                        flip.flameFirepointRight.transform.position.z);
                }
            }

            //Click mouse button to fire
            if (Input.GetMouseButton(0) && gunScriptableObject.canFire)
            {
                AnimGunRight.SetBool("Fire", true);
                AnimGunLeft.SetBool("Fire", true);
                AnimPlayer.SetBool("Fire", true);
                gunScriptableObject.canFire = false;
                //Debug.Log("Shot bullet");
                //this causes this screen to shake when you shoot
                StartCoroutine(cameraScript.Shake());
                Instantiate(gunScriptableObject.bullet, gunScriptableObject.bulletTransform, Quaternion.identity);
                GunFire.Play();
                //Debug.Log("Playing Sound");


                //counts down ammo
                gunScriptableObject.ammoCountCurrent = gunScriptableObject.ammoCountCurrent - 1;
                //Debug.Log("playing shooting");

                if (heldDown == false) heldDown = true;
            }
            if(heldDown = true && Input.GetMouseButtonUp(0))
            {
                
                AnimGunRight.SetBool("Fire", false);
                AnimGunLeft.SetBool("Fire", false);
                AnimPlayer.SetBool("Fire", false);
                //Debug.Log("not playing shooting");
                heldDown = false;
                
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
        gunScriptableObject.reloading = true;
        AnimGunRight.SetBool("Reload", true);
        AnimGunLeft.SetBool("Reload", true);
        gunScriptableObject.canFire = false;
        yield return new WaitForSeconds(gunScriptableObject.reloadTimer);
        gunScriptableObject.ammoCountCurrent = gunScriptableObject.ammoCountMax;
        gunScriptableObject.reloading = false;
        AnimGunRight.SetBool("Reload", false);
        AnimGunLeft.SetBool("Reload", false);
        gunScriptableObject.canFire = true;
        
    }
}
