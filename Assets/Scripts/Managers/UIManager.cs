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

    private Character Player => GameManager.Instance.Player;

    private WeaponMasterModule weaponMaster;

    public WeaponModule CurrentWeapon => weaponMaster.CurrentWeapon;
    #endregion

    #region Properties
    //private Character Player => GameManager.Instance.Player;

    //public WeaponMasterModule WeaponMaster => GameManager.Instance.PlayerWeapons;

    //public WeaponModule CurrentWeapon => WeaponMaster.CurrentWeapon;
    #endregion

    #region Init
    //Awake is called before Start
    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Sets current scene variables
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        weaponMaster = Player.gameObject.transform.GetComponentInChildren<WeaponMasterModule>();
        weaponMaster.onSwitchToWeapon.AddListener(OnSwitchWeapon);
    }
    #endregion

    private void OnSwitchWeapon(WeaponModule weapon)
    {
        ammoUI.sprite = weapon.weaponUIImage;
    }

    #region Main Loop
    // Update is called once per frame
    private void Update()
    {
        //Check if instance is not null, if it is not, do update stuff
        if (instance != null)
        {
            // Alt fire slider
            altSlider.value = CurrentWeapon.altFireDelay.Percent;

            // Health
            healthSlider.value = Player.HPPercent;

            // Sets ammo count
            ammoCount.text = CurrentWeapon.currentAmmo.ToString() + "/" + CurrentWeapon.ammoCount.ToString();

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
