using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
    public Text ammoCount;
    public Slider healthSlider;
    public Slider dashSlider;
    public Slider altSlider;
    //public float ammoCur;
    //public float ammoMax;
    //public float healthCur; Can get these two fields from the player health script via public reference
    //public float healthMax = 1;

    //this is for the BurningScript. I needed to put it somewhere else so that it could be referenced and changed by the burning script
    public bool boxBurning = false;
    public bool isPaused = false;

    public CharacterMovementModule player;

    public RespawnScript respawn;

    public WeaponMasterModule weaponMaster;

    private WeaponModule currWeapon;

    public GameObject pauseMenu;

    //public float altTime;
    //public float timestamp;


    public GameObject tommyAmmoUI;
    public GameObject shotgunAmmoUI;
    public GameObject flameAmmoUI;

    //public TommyGunWeapon tommyScript;
    //public ShotGunWeapon shotgunScript;
    //public FlameThrowerWeapon flamethrowerScript;

    public bool isDying = false;

    //public Camera camera;

    private List<GameObject> ammoUIList = new List<GameObject>();

    private GameObject currAmmoUI;

    

    //private Duration altDuration = new(1f);

    #region Instance
    /// <summary>
    /// The static reference to an EventManager instance.
    /// </summary>
    private static UIManager instance;

    /// <summary>
    /// The static reference to an EventManager instance.
    /// </summary>
    public static UIManager Instance => instance;
    #endregion

    #region Properties
    //public GameObject CurrentAmmoUI
    //{
    //    get { return currAmmoUI; }

    //    set
    //    {
            
    //    }
    //}
    #endregion

    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the ammo UI according to the current weapon in the weaponmaster, way below will be changed

        ammoUIList.AddRange(new List<GameObject> { tommyAmmoUI, shotgunAmmoUI, flameAmmoUI});

        Time.timeScale = 1;

        pauseMenu.SetActive(false);

        currWeapon = weaponMaster.CurrentWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if instance is not null, if it is not, do update stuff
        if (instance != null)
        {
            //Caching the current weapon
            if (weaponMaster.CurrentWeapon != currWeapon)
            {
                currWeapon = weaponMaster.CurrentWeapon;
                //Sets weapon UI when weapon is re-cached
                switch (currWeapon.weaponName)
                {
                    case "shotgun":
                        SetActiveAmmo(shotgunAmmoUI);
                        break;
                    case "flamethrower":
                        SetActiveAmmo(flameAmmoUI);
                        break;
                    default:
                        SetActiveAmmo(tommyAmmoUI);
                        break;
                }
            }


            //Alt fire slider
            if (currWeapon.altFireDelay.IsDone)
            {
                altSlider.value = 1f;
            }
            else
            {
                altSlider.value = currWeapon.altFireDelay.elapsed / currWeapon.altFireDelay.maxTime;
            }


            //Input stuff here is for testing.
            if (Input.GetKeyDown(KeyCode.J))
            {
                healthSlider.value += .1f;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                healthSlider.value -= .1f;
            }

            //ammoCount.text = ammoCur.ToString() + "/" + ammoMax.ToString();
            ammoCount.text = currWeapon.currentAmmo.ToString() + "/" + currWeapon.ammoCount.ToString();

            /**The UIManager should not be responsible for killing the player
             * The part of this code actually handling the player death should be in a different script
             * The stuff handling the UI should be here, with it either being called by the other script or
             * having the UI call the other script to initiate player death**/

            //Set the healthSlider.value to (current player health/max player health)

            if (healthSlider.value == 0)
            {
                //All of this needs to be in the player script, not the UI
                //player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //player.isKBed = false;
                //isDying = true;
                ////mainPlayer.layer = 10;
                //AudioDictionary.aDict.PlayAudioClip("playerDeath", AudioDictionary.Source.Player);
                //animator.Play("PlayerDeath");
                //animatorGun.Play("GunOff");
                ////animatorGunLeft.Play("GunOff");
                ////Time.timeScale = 0.25f;
                //StartCoroutine(deathExplosion());

                healthSlider.value = 1;

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    /**This function should not be here, it should be in a script that handles either player movement
     * or game events. **/

    //public void RespawnDelay()
    //{
    //    respawn.respawnPlayer();
    //    animator.Play("PizzaGuy_Idle");
    //    animatorGun.Play("Idle");
    //    Time.timeScale = 1;
    //}

    //Should be fine, might just need cleanup but will double check

    //the dash needs to refill based on the dodgeCooldown in the playerMovement script, which atm is 2f
    public IEnumerator dashFill()
    {
        dashSlider.value = 0f;
        yield return new WaitForSeconds(.5f);
        dashSlider.value = 0.25f;
        yield return new WaitForSeconds(.5f);
        dashSlider.value = 0.5f;
        yield return new WaitForSeconds(.5f);
        dashSlider.value = 0.75f;
        yield return new WaitForSeconds(.5f);
        dashSlider.value = 1f;
    }


    //Also I think will just need cleanup, but we will see
    //public IEnumerator deathExplosion()
    //{
    //    yield return new WaitForSeconds(0.55f);
    //    CameraShake shake = camera.GetComponent<CameraShake>();
    //    shake.shakeDuration = 0.5f;
    //    yield return new WaitForSeconds(0.55f);
    //    Invoke(nameof(RespawnDelay), 0.6f);
    //}


    private void SetActiveAmmo(GameObject ammoUI)
    {
        foreach (GameObject ammo in ammoUIList)
        {
            ammo.gameObject.SetActive(false);
            ammoUI.SetActive(true);
        }
    }

    private void PauseGame()
    {
        if (pauseMenu.activeSelf == true)
        {
            //Unpauses game
            pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else if (pauseMenu.activeSelf == false)
        {
            //Pauses game
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }

    }


}
