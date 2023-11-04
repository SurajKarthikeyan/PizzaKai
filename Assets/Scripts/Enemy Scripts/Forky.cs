using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Responsible for this file: Zane O'Dell
/// 
/// This class will handle the logic for Forky, including animations, sounds and instantiating boxes
/// 
/// <br>
/// 
/// Authors: Zane O'Dell
/// </summary>
public class Forky : MonoBehaviour
{
    #region Variables

    [Header("Audio Settings")]
    [Tooltip("Audio Source for forky")]
    public AudioSource forkyAudioSource;
    [Tooltip("Audio Source for forky walkie talkie")]
    public AudioSource walkieTalkieSource;

    [Header("Forky Boss Logic")]

    [Tooltip("Is Forky Active")]
    public bool active = false;

    public bool spawning = false;

    [Header("Spawn interval settings")]

    [Tooltip("Minimum time for forkyr2d to spawn")]
    [SerializeField]
    private float minBoxSpawnTime = 5f;

    [Tooltip("Maximum time for forkyr2d to spawn")]
    [SerializeField]
    private float maxBoxSpawnTime = 7f;

    
    [Tooltip("Minimum time for a series of enemies to spawn")]
    [SerializeField]
    private float minEnemySpawnTime = 6f;

    [Tooltip("Maximum time for a series of enemies to spawn")]
    [SerializeField]
    private float maxEnemySpawnTime = 9f;

    
    [Tooltip("Maximum number of enemies spawned at a time")]
    [SerializeField]
    private int maxEnemyCount = 2;

    
    [Tooltip("Minimum height of boxes spawned at any given time")]
    [SerializeField]
    private int minBoxHeight = 1;

    [Tooltip("Maximum height of boxes spawned at any given time")]
    [SerializeField]
    private int maxBoxHeight = 4;

    //Chance of oil spawning
    private float oilChance = 0;

    //Has an action been taken by Forky yet?
    public bool actionTaken = false;

    [Header("EnemySpawnTransform")]

    public Transform enemySpawnPoint;

    //Crate generator script reference
    private ForkyCrateSpawner crateSpawner;

    [Header("Breadstick enemy to spawn")]
    public GameObject breadstick;

    [SerializeField] 
    private ConveyorBeltScript conveyorBelt;

    private int generators = 3;


    [Tooltip("Is Forky Dead")]
    [NaughtyAttributes.ReadOnly]
    public bool IsDead = false;


    public GameObject pipes;

    [SerializeField]
    private LayerMask enemyLayer;

    //Change to forky
    public Rigidbody2D forkyr2d;

    public Tilemap tilemap;

    public GameObject dyingForky;

    #endregion

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        crateSpawner = GetComponent<ForkyCrateSpawner>();
    }
    #endregion

    #region Main Loop
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (IsDead)
            {
                //Play Death Animation once and destroy object
                spawning = false;
                StopAllCoroutines();
                StartCoroutine(KillForky());
            }

            //Logic for when the boss is alive
            else if (!actionTaken)
            {
                if (spawning)
                {
                    float enemySpawnChance = Random.Range(0, 1f);
                    if (enemySpawnChance <= 0.15f)
                    {
                        //Start coroutine for spawning enemies after a certain amount of time
                        StartCoroutine(IntervalSpawner(true));
                    }
                    else
                    {
                        //Start coroutine for spawning boxes
                        StartCoroutine(IntervalSpawner(false));
                    }
                }
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Coroutine that continually runs and spawns enemies and boxes 
    /// </summary>
    /// <param name="spawnEnemies">Bool to decide whether eneimes or boxes are spawned</param>
    /// <returns></returns>
    IEnumerator IntervalSpawner(bool spawnEnemies)
    {
        //We say an action has been taken, spawning boxes or enemies
        actionTaken = true;
        if (spawnEnemies)
        {
            //Next lines are responsible for calculating the duration and number of enemies to spawn
            float enemySpawnDuration = Random.Range(minEnemySpawnTime, maxEnemySpawnTime);
            yield return new WaitForSeconds(enemySpawnDuration);
            int enemyNum = Random.Range(1, maxEnemyCount);
            float chance = Random.Range(0f, 1f);

            // Play one of two Radio SFX
            if (chance < 0.5f)
            {
                AudioDictionary.aDict.PlayAudioClipRemote("forkyRadio", walkieTalkieSource);
            }

            else
            {
                AudioDictionary.aDict.PlayAudioClipRemote("forkyRadio1", walkieTalkieSource);
            }

            //Spawns enemies in intervals of 1 second
            while (enemyNum >= 0)
            {
                Instantiate(breadstick, enemySpawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
                enemyNum--;
            }

        }
        else
        {
            //Calculates interval and number of boxes to spawn
            float boxSpawnDuration = Random.Range(minBoxSpawnTime, maxBoxSpawnTime);
            yield return new WaitForSeconds(boxSpawnDuration);
            int boxNum = Random.Range(minBoxHeight, maxBoxHeight + 1);
            //Spawn boxes with potential oil barrel

            bool oil = Random.Range(0, 101) <= oilChance;

            //Spawns number of crates
            crateSpawner.SpawnCrate(boxNum, oil);

            //Increases oil chance if it is not spawned, of if spawned reset chance
            if(!oil)
            {
                oilChance += 25;
            }
            else
            {
                oilChance = 0;
            }

            AudioDictionary.aDict.PlayAudioClipRemote("forkLift", forkyAudioSource);
        }
        actionTaken = false;
    }

    /// <summary>
    /// Coroutine that kills forky
    /// </summary>
    /// <returns></returns>
    IEnumerator KillForky()
    {
        //Makes pipes fall behind Forky
        Rigidbody2D[] rigidBodies = pipes.GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < rigidBodies.Length; i++)
        {
            Rigidbody2D rigidBody = rigidBodies[i];
            rigidBody.gravityScale = 1;
        }

        //Kills all enemies
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 25, enemyLayer);
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            Destroy(enemyColliders[i].gameObject);
        }
        active = false;
        yield return new WaitForSeconds(1.5f);
        CinemachineCameraShake.instance.ShakeScreen(60f, 1f);
        if (forkyr2d != null && forkyr2d.gameObject != null)
        {
            yield return new WaitForSeconds(0.3f);
            forkyr2d.gameObject.GetComponent<Animator>().SetBool("Dying", true);
            forkyr2d.gravityScale = 1f;
            forkyr2d.velocity = Vector3.left * 7 + Vector3.up * 5;
            yield return new WaitForSeconds(0.5f);
            forkyr2d.velocity = Vector3.zero;
            forkyr2d.gameObject.layer = 14;
            BoxCollider2D boxCollider = forkyr2d.GetComponentInParent<BoxCollider2D>();
            boxCollider.enabled = true;
            forkyr2d.gravityScale = 10f;
        }

    }

    public void HelpAfterForky()
    {
        StartCoroutine(AfterForkyDeath());
    }

    /// <summary>
    /// After forky dies (RIP)
    /// </summary>
    /// <returns></returns>
    public IEnumerator AfterForkyDeath()
    {
        tilemap.animationFrameRate = 0;
        conveyorBelt.conveyorSpeed = 0;
        Vector2 explosionPos = new(dyingForky.transform.position.x - 0.65f, gameObject.transform.position.y);
        ExplosionManager.Instance.SelectExplosionRandom(explosionPos, -90);
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.CallDialogueBlock("Post-Forky Fight");
    }

    /// <summary>
    /// A function that sets variables
    /// </summary>
    public void NextPhase()
    {

        //Changing dependent variables for spawning logic and remove the generator
        generators -= 1;

        if(generators == 0)
        {
            tilemap.animationFrameRate = 2;
            conveyorBelt.conveyorSpeed = -2;
            IsDead = true;
            return;
        }

        //Increase rate of spawning objects
        maxBoxSpawnTime -= 1;
        minBoxSpawnTime -= 1;
        maxEnemySpawnTime -= 1;
        minEnemySpawnTime -= 1;

        //Add or subtract 1 to the conveyor speed depending on if the conveyorbelt has a negative or positive speed
        conveyorBelt.conveyorSpeed = (Mathf.Abs(conveyorBelt.conveyorSpeed) + 1) * conveyorBelt.conveyorSpeed / Mathf.Abs(conveyorBelt.conveyorSpeed);
        if (generators == 1)
        {
            tilemap.animationFrameRate = 3;
        }
        else
        {
            tilemap.animationFrameRate = 2;
        }
    }
    #endregion
}
