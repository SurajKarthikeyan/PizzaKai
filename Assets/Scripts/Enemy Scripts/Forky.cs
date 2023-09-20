using System.Collections;
using System.Collections.Generic;
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

    public AudioSource forkyAudioSource;

    public Animator forkyAnimator;

    private float minBoxSpawnTime = 5f;

    private float maxBoxSpawnTime = 7f;

    private float minEnemySpawnTime = 6f;

    private float maxEnemySpawnTime = 9f;

    private int maxEnemyCount = 2;

    private int minBoxHeight = 1;

    private int maxBoxHeight = 4;

    private float oilChance = 0;

    private bool containsOil = false;

    private bool actionTaken = false;

    public GameObject boxSpawnPoint;

    public GameObject enemySpawnPoint;

    //Crate generator script reference
    ForkyCrateSpawner crateSpawner;

    public GameObject breadstick;

    public List<GameObject> generators = new List<GameObject>();
    #endregion

    #region Properties
    //C# property. Acts as a "getter" of sorts. Returns true when count <= 0, false otherwise
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
            SceneManager.LoadScene("MainMenu");
        }

        //Logic for when the boss is alive
        else if (!actionTaken)
        {
            float enemySpawnChance = Random.Range(0, 1f);
            if (enemySpawnChance <= -1f) // was 0.25f
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
    IEnumerator IntervalSpawner(bool spawnEnemies)
    {
        actionTaken = true;
        if (spawnEnemies)
        {
            float enemySpawnDuration = Random.Range(minEnemySpawnTime, maxEnemySpawnTime);
            yield return new WaitForSeconds(enemySpawnDuration);
            int enemyNum = Random.Range(1, maxEnemyCount);
            while (enemyNum >= 0)
            {
                Instantiate(breadstick, enemySpawnPoint.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
                enemyNum--;
            }

        }
        else
        {
            float boxSpawnDuration = Random.Range(minBoxSpawnTime, maxBoxSpawnTime);
            yield return new WaitForSeconds(boxSpawnDuration);
            int boxNum = Random.Range(minBoxHeight, maxBoxHeight);
            //Spawn boxes with potential oil barrel

            bool oil = Random.Range(0, 101) <= oilChance;

            crateSpawner.SpawnCrate(boxNum, true);  // change this back to oil instead of true dingus

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

    public void NextPhase(GameObject generator)
    {
        //Changing dependent variables for spawning logic and remove the generator
        generators.Remove(generator);
        if (generators.Count == 1)
        {
            maxEnemyCount++;
        }
        else if(generators.Count == 0)
        {
            IsDead = true;
        }
        maxBoxSpawnTime -= 1;
        minBoxSpawnTime -= 1;
        maxEnemySpawnTime -= 1;
        minEnemySpawnTime -= 1;
    }
    #endregion
}
