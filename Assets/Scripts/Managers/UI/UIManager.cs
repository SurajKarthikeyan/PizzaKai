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

    #region Variables
    [Header("UI Text Fields and Sliders")]
    [Tooltip("Text used to show current weapon ammo")]
    public Text ammoCount;

    [Tooltip("Image of the ammo type.")]
    [SerializeField]
    private Image ammoUI;

    [Tooltip("Slider used to show the player health")]
    public Slider healthSlider;

    [Tooltip("Slider used to show the player's dash cooldown")]
    public Slider dashSlider;

    [Tooltip("Slider used to show the player's dash cooldown")]
    public Slider altSlider;

    [Header("UI and game screens/elements")]
    [Header("UI relevant player information")]
    [Tooltip("Script containing UI relevant player information")]
    public Character player;

    [Tooltip("Weapon master module to use for UI")]
    public WeaponMasterModule weaponMaster;
    #endregion

    #region Init
    //Awake is called before Start
    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);

        if (!player)
        {
            GameObject.FindGameObjectWithTag("Player")
                .RequireComponent(out player);
        }

        if (!weaponMaster)
        {
            player.RequireComponentInChildren(out weaponMaster);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1;
    }
    #endregion

    #region Main Loop
    private void Update()
    {
        var weap = weaponMaster.CurrentWeapon;

        // Update graphics. Primary ammo first.
        ammoUI.sprite = weap.ammoGraphic;
        ammoCount.text = $"{weap.currentAmmo}/{weap.ammoCount}";

        // Alt recharge.
        altSlider.value = weap.altFireDelay.Percent;

        // Dash recharge.
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
    public IEnumerator DashFill()
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


    // //This should also be with the player
    // //public IEnumerator deathExplosion()
    // //{
    // //    yield return new WaitForSeconds(0.55f);
    // //    CameraShake shake = camera.GetComponent<CameraShake>();
    // //    shake.shakeDuration = 0.5f;
    // //    yield return new WaitForSeconds(0.55f);
    // //    Invoke(nameof(RespawnDelay), 0.6f);
    // //}

    // /// <summary>
    // /// Pauses and unpauses the game
    // /// </summary>
    // private void PauseGame()
    // {
    //     if (pauseMenu.activeSelf == true)
    //     {
    //         //Unpauses game
    //         pauseMenu.SetActive(false);
    //         isPaused = false;
    //         Time.timeScale = 1;
    //     }
    //     else if (pauseMenu.activeSelf == false)
    //     {
    //         //Pauses game
    //         pauseMenu.SetActive(true);
    //         isPaused = true;
    //         Time.timeScale = 0;
    //     }

    // }
    #endregion
}
