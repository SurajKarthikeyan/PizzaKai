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

    [Header("UI relevant player information")]
    [Tooltip("Script containing UI relevant player information")]
    public Character player;

    [Tooltip("bool telling if player is dying. (this needs to go/be fixed)")]
    public bool isDying;

    [Tooltip("Weapon master module to use for UI")]
    public WeaponMasterModule weaponMaster;

    private WeaponModule currWeapon;
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

        currWeapon = weaponMaster.CurrentWeapon;

        ammoUI.sprite = currWeapon.weaponUIImage;
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
                ammoUI.sprite = currWeapon.weaponUIImage;
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


            //Sets ammo count and health

            healthSlider.value = (float)player.HP / (float)player.maxHP;

            ammoCount.text = currWeapon.currentAmmo.ToString() + "/" + currWeapon.ammoCount.ToString();

            if (healthSlider.value == 0)
            {
                healthSlider.value = 1;
            }
            if(!player.GetComponent<CharacterMovementModule>().canDash)
            {
                DashFill();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }

            //This check should not be here but frankly I don't care rn
            if (!DialogueManager.Instance.levelFlowchart.HasExecutingBlocks() && pauseMenu.activeSelf == false)
            {
                
                if (!weaponMaster.weaponsAvailable)
                {
                    DialogueManager.Instance.StopPlayer(false);
                    weaponMaster.CurrentWeapon.gameObject.SetActive(false);
                }
                else
                {
                    if (!weaponMaster.enabled)
                    {
                        weaponMaster.enabled = true;
                    }
                }
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
        dashSlider.value = Mathf.InverseLerp(0, player.GetComponent<CharacterMovementModule>().dashCooldown,
            Time.time - player.GetComponent<CharacterMovementModule>().dashTime);
    }

    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenu.activeSelf == true)
        {
            //Unpauses game
            pauseMenu.SetActive(false);
            if (!DialogueManager.Instance.levelFlowchart.HasExecutingBlocks())
            {
                DialogueManager.Instance.StopPlayer(false);
            }
            if (!weaponMaster.weaponsAvailable)
            {
                weaponMaster.CurrentWeapon.gameObject.SetActive(false);
            }
            isPaused = false;
            Time.timeScale = 1;
        }
        else if (pauseMenu.activeSelf == false)
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
