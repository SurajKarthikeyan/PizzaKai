using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    //Spawn interval settings

    //Minimum time for box to spawn
    private float minBoxSpawnTime = 5f;

    //Maximum time for box to spawn
    private float maxBoxSpawnTime = 7f;

    //Minimum time for a series of enemies to spawn
    private float minEnemySpawnTime = 6f;
    //Maximum time for a series of enemies to spawn
    private float maxEnemySpawnTime = 9f;

    //Maximum number of enemies spawned at a time
    private int maxEnemyCount = 2;

    //Minimum height of boxes spawned at any given time
    private int minBoxHeight = 1;

    //Maximum height of boxes spawned at any given time
    private int maxBoxHeight = 4;

    //Chance of oil spawning
    private float oilChance = 0;
    //Has an action been taken by Forky yet?
    private bool actionTaken = false;

    [Header("EnemySpawnTransform")]

    public Transform enemySpawnPoint;

    //Crate generator script reference
    ForkyCrateSpawner crateSpawner;

    public GameObject breadstick;

    [SerializeField] private ConveyorBeltScript conveyorBelt;

    int generators = 3;

    public bool IsDead = false;
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
        if (IsDead)
        {
            //Play Death Animation once and destroy object
            //Going to Main Menu is temporary
            SceneManager.LoadScene("MainMenu");
        }

        //Logic for when the boss is alive
        else if (!actionTaken)
        {
            float enemySpawnChance = Random.Range(0, 1f);
            if (enemySpawnChance <= 0.25f) 
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
    /// A function that sets variables
    /// </summary>
    public void NextPhase()
    {

        //Changing dependent variables for spawning logic and remove the generator
        generators -= 1;

        if(generators == 0)
        {
            IsDead = true;
            conveyorBelt.conveyorSpeed = 0;
        }

        maxBoxSpawnTime -= 1;
        minBoxSpawnTime -= 1;
        maxEnemySpawnTime -= 1;
        minEnemySpawnTime -= 1;

        //Add or subtract 1 to the conveyor speed depending on if the conveyorbelt has a negative or positive speed
        conveyorBelt.conveyorSpeed = (Mathf.Abs(conveyorBelt.conveyorSpeed) + 1) * conveyorBelt.conveyorSpeed / Mathf.Abs(conveyorBelt.conveyorSpeed);
    }
    #endregion
}
