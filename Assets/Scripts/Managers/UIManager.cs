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

    [Tooltip("Image containing the ammo UI element")]
    public Image ammoUI;

    public WeaponMasterModule weaponMaster;

    private WeaponModule currentWeapon;
    #endregion

    #region Properties
    private Character Player => GameManager.Instance.Player;
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
        //Sets current scene variables
        Time.timeScale = 1;

        pauseMenu.SetActive(false);

        ammoUI.sprite = currentWeapon.weaponUIImage;
    }
    #endregion

    #region Main Loop
    // Update is called once per frame
    private void Update()
    {
        //Check if instance is not null, if it is not, do update stuff
        if (instance != null)
        {
            // //Caching the current weapon
            if (weaponMaster.CurrentWeapon != currentWeapon)
            {
                currentWeapon = weaponMaster.CurrentWeapon;
                //Sets weapon UI when weapon is re-cached
                ammoUI.sprite = currentWeapon.weaponUIImage;
            }

            //Alt fire slider
            if (currentWeapon.altFireDelay.IsDone)
            {
                altSlider.value = 1f;
            }
            else
            {
                altSlider.value = currentWeapon.altFireDelay.elapsed / currentWeapon.altFireDelay.maxTime;
            }


            //Sets ammo count and health

            healthSlider.value = Player.HP / (float)Player.maxHP;

            ammoCount.text = currentWeapon.currentAmmo.ToString() + "/" + currentWeapon.ammoCount.ToString();

            if (healthSlider.value == 0)
            {
                healthSlider.value = 1;
            }
            if (!GameManager.Instance.PlayerMovement.dashCooldown.IsDone)
            {
                DashFill();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// Fills the dash meter
    /// </summary>
    public void DashFill()
    {
        //Fills the dash slider over times
        dashSlider.value = Mathf.InverseLerp(
            0,
            GameManager.Instance.PlayerMovement.dashCooldown.maxTime,
            GameManager.Instance.PlayerMovement.dashCooldown.elapsed
        );
    }

    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
    private void PauseGame()
    {
        if (pauseMenu.activeSelf)
        {
            //Unpauses game
            pauseMenu.SetActive(false);
            DialogueManager.Instance.StopPlayer(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            //Pauses game
            pauseMenu.SetActive(true);
            DialogueManager.Instance.StopPlayer(true);
            isPaused = true;
            Time.timeScale = 0;
        }

    }
    #endregion
}
