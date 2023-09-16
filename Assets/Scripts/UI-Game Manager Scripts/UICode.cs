using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICode : MonoBehaviour
{
    public Text AmmoCount;
    public Slider healthSlider;
    public Slider dashSlider;
    public Slider altSlider;
    public float ammoCur;
    public float ammoMax;
    public float healthCur;
    public float healthMax = 1;
    public float healthLoss;

    //this is for the BurningScript. I needed to put it somewhere else so that it could be referenced and changed by the burning script
    public bool boxBurning = false;
    public bool isPaused = false;

    public bool tommyEquipped = false;
    public bool shotgunEquipped = false;
    public bool flameEquipped = false;

    public PlayerMovement player;
    
    public RespawnScript respawn;

    public AudioSource PlayerDeath;
    public AudioClip PizzaDeath;

    public Animator animator;
    private GameObject pizzaPlayer;

    public Animator animatorGun;
    private GameObject pizzaArm;

//    public Animator animatorGunLeft;
  //  private GameObject pizzaArmLeft;

    public GameObject pauseMenu;

    public float altTime;
    public float timestamp;

    public GameObject mainPlayer;

    public GameObject ammoTommy;
    public GameObject ammoShotgun;
    public GameObject ammoFlame;
    public GameObject ammoSniper;

    public GameObject tommy;
    public GameObject shotgun;
    public GameObject flamethrower;
    public GameObject sniper;
    public GameObject tommyFlash;
    public Image tommyFlashImage;

    public Shooting shootingTommy;
    public Shooting shootingShotgun;
    public Shooting shootingFlamethrower;
    public Shooting shootingSniper;

    public TommyGun tommyScript;
    public Shotgun shotgunScript;
    public Flamethrower flamethrowerScript;
    public Sniper sniperScript;

    public bool isFlashing = false;
    public bool isDying = false;

    private Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        tommy.SetActive(true);
        
        shotgun.SetActive(false);
        flamethrower.SetActive(false);

        Time.timeScale = 1;
        healthCur = healthMax;
        
        pauseMenu.SetActive(false);

        pizzaPlayer = GameObject.FindWithTag("PlayerSprite");
        animator = pizzaPlayer.GetComponent<Animator>();

        pizzaArm = GameObject.FindWithTag("PlayerArm");
        animatorGun = pizzaArm.GetComponent<Animator>();

        Camera = GetComponentInChildren<Camera>();

        //pizzaArmLeft = GameObject.FindWithTag("PlayerArmLeft");
        //animatorGunLeft = pizzaArmLeft.GetComponent<Animator>();
        // the above should be "pizzaArmLeft." ... but doing so causes errors
        // because the game can't find pizzaArmLeft on Start because it starts
        // off, and the game will never find the animatorGunLeft. I think the
        // TommyGun arms need to start on and move this to awake before they
        // get set off, so that they can be referenced still. I also imagine
        // this code will never get the other arms for Shotgun or Flamethrower,
        // since it will only ever be able to fight the RightTommyArm as this
        // is not in Update either.

    }

    // Update is called once per frame
    void Update()
    {
        pizzaArm = GameObject.FindWithTag("PlayerArm");
        animatorGun = pizzaArm.GetComponent<Animator>();

        if (timestamp / altTime == 0f)
        {
            altSlider.value = 1f;
        } else
        {
            altSlider.value = timestamp / altTime;
        }
        
        
        AmmoCount.text = ammoCur.ToString() + "/" + ammoMax.ToString();
        

        if (healthSlider.value == 0)
        {
            player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.isKBed = false;
            isDying = true;
            //mainPlayer.layer = 10;
            PlayerDeath.Play();
            animator.Play("PlayerDeath");
            animatorGun.Play("GunOff");
            //animatorGunLeft.Play("GunOff");
            //Time.timeScale = 0.25f;
            StartCoroutine(deathExplosion());
        
            healthSlider.value = 1;
            
        }

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
            } else if (pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
        }

        if (tommy.activeSelf == true)
        {
            altTime = tommyScript.gunScriptableObject.timeTillAlt;
            timestamp = tommyScript.gunScriptableObject.timestamp;
            ammoMax = shootingTommy.gunScriptableObject.ammoCountMax;
            ammoCur = shootingTommy.gunScriptableObject.ammoCountCurrent;
            ammoTommy.SetActive(true);
            ammoShotgun.SetActive(false);
            ammoFlame.SetActive(false);
            ammoSniper.SetActive(false);
        }

        else if (shotgun.activeSelf == true)
        {
            altTime = shotgunScript.gunScriptableObject.timeTillAlt;
            timestamp = shotgunScript.gunScriptableObject.timestamp;
            ammoMax = shootingShotgun.gunScriptableObject.ammoCountMax;
            ammoCur = shootingShotgun.gunScriptableObject.ammoCountCurrent;
            ammoTommy.SetActive(false);
            ammoShotgun.SetActive(true);
            ammoFlame.SetActive(false);
            ammoSniper.SetActive(false);
        }

        else if (flamethrower.activeSelf == true)
        {
            altTime = flamethrowerScript.gunScriptableObject.timeTillAlt;
            timestamp = flamethrowerScript.gunScriptableObject.timestamp;
            ammoMax = shootingFlamethrower.gunScriptableObject.ammoCountMax;
            ammoCur = shootingFlamethrower.gunScriptableObject.ammoCountCurrent;
            ammoTommy.SetActive(false);
            ammoShotgun.SetActive(false);
            ammoFlame.SetActive(true);
            ammoSniper.SetActive(false);
        }
        else if (sniper.activeSelf == true)
        {
            altTime = sniperScript.gunScriptableObject.timeTillAlt;
            timestamp = sniperScript.gunScriptableObject.timestamp;
            ammoMax = sniperScript.gunScriptableObject.ammoCountMax;
            ammoCur = sniperScript.gunScriptableObject.ammoCountCurrent;
            ammoTommy.SetActive(false);
            ammoShotgun.SetActive(false);
            ammoFlame.SetActive(false);
            ammoSniper.SetActive(true);
        }

    }

    public void RespawnDelay()
    {
        respawn.RespawnPlayer();
        animator.Play("PizzaGuy_Idle");
        animatorGun.Play("Idle");
        Time.timeScale = 1;
    }

    public void GainHealth (float health)
    {
        healthSlider.value = healthSlider.value + health;
    }

    public void LoseHealth (float health)
    {
        healthSlider.value = healthSlider.value - health;
    }

    
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

    public IEnumerator tommyAltFlash()
    {
        tommyFlash.SetActive(true);
        isFlashing = true;
        tommyFlashImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.8f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.6f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(0.2f);
        tommyFlashImage.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        tommyFlash.SetActive(false);
        isFlashing = false;
    }

    public IEnumerator deathExplosion()
    {
        yield return new WaitForSeconds(0.55f);
        CameraShake shake = Camera.GetComponent<CameraShake>();
        shake.shakeDuration = 0.5f;
        yield return new WaitForSeconds(0.55f);
        Invoke(nameof(RespawnDelay), 0.6f);
    }


    
}
