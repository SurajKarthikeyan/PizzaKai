using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager class that manages UI interactions and information displaying
/// 
/// <br/>
/// 
/// Authors: Zane O'Dell (2023)
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("UI Text Fields and Sliders")]
    [Tooltip("Text used to show current weapon ammo")]
    public Text ammoCount;

    [Tooltip("Slider used to show the player health")]
    public Slider healthSlider;

    [Tooltip("Slider used to show the player's dash cooldown")]
    public Slider dashSlider;

    [Tooltip("Slider used to show the player's dash cooldown")]
    public Slider altSlider;

    [Header("UI and game screens/elements")]
    [Tooltip("Boolean representing whether the game is paused or not")]
    public bool isPaused = false;

    [Tooltip("GameObject representing the pause menu")] 
    public GameObject pauseMenu;

    [Tooltip("GameObject containing the tommy ammo UI element")]
    public GameObject tommyAmmoUI;

    [Tooltip("GameObject containing the shotgun ammo UI element")]
    public GameObject shotgunAmmoUI;

    [Tooltip("GameObject containing the flamethrower ammo UI element")]
    public GameObject flameAmmoUI;

    [Tooltip("Slider used to show the player's dash cooldown")]
    public RespawnScript respawn;

    [Header("UI relevant player information")]
    [Tooltip("Script containing UI relevant player information")]
    public CharacterMovementModule player;

    [Tooltip("bool telling if player is dying. (this needs to go/be fixed)")]
    public bool isDying;

    [Tooltip("Weapon master module to use for UI")]
    public WeaponMasterModule weaponMaster;

    private WeaponModule currWeapon;

    private List<GameObject> ammoUIList = new List<GameObject>();
    #endregion

    #region Instance
    /// <summary>
    /// The static reference to a UIManager instance.
    /// </summary>
    private static UIManager instance;

    /// <summary>
    /// The static reference to a UIManager instance.
    /// </summary>
    public static UIManager Instance => instance;
    #endregion

    #region Init
    //Awake is called before Start
    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the ammo UI according to the current weapon in the weaponmaster, way below will be changed

        ammoUIList.AddRange(new List<GameObject> { tommyAmmoUI, shotgunAmmoUI, flameAmmoUI });

        Time.timeScale = 1;

        pauseMenu.SetActive(false);

        currWeapon = weaponMaster.CurrentWeapon;
    }
    #endregion

    #region Main Loop
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


            //TODO: Set the healthSlider.value to (current player health/max player health)
            //Input stuff here is for testing.
            if (Input.GetKeyDown(KeyCode.J))
            {
                healthSlider.value += .1f;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                healthSlider.value -= .1f;
            }

            ammoCount.text = currWeapon.currentAmmo.ToString() + "/" + currWeapon.ammoCount.ToString();

            if (healthSlider.value == 0)
            {
                /**The UIManager should not be responsible for killing the player
             * The part of this code actually handling the player death should be in a different script
             * The stuff handling the UI should be here, with it either being called by the other script or
             * having the UI call the other script to initiate player death**/
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
    #endregion

    #region Methods
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

    //the dash needs to refill based on time passed in, right now it's "hard coding" two seconds
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


    //This should also be with the player
    //public IEnumerator deathExplosion()
    //{
    //    yield return new WaitForSeconds(0.55f);
    //    CameraShake shake = camera.GetComponent<CameraShake>();
    //    shake.shakeDuration = 0.5f;
    //    yield return new WaitForSeconds(0.55f);
    //    Invoke(nameof(RespawnDelay), 0.6f);
    //}

    /// <summary>
    /// Sets the active ammo UI element for the gun
    /// </summary>
    /// <param name="ammoUI">UI ammo element to set</param>
    private void SetActiveAmmo(GameObject ammoUI)
    {
        foreach (GameObject ammo in ammoUIList)
        {
            ammo.gameObject.SetActive(false);
            ammoUI.SetActive(true);
        }
    }
    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
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
    #endregion
}
