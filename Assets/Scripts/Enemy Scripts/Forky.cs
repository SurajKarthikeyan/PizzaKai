using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private Duration enemySpawnDuration;

    private Duration boxSpawnDuration;

    private float minBoxSpawnTime = 5f;

    private float maxBoxSpawnTime = 7f;

    private float minEnemySpawnTime = 6f;

    private float maxEnemySpawnTime = 9f;

    private float minBoxHeight = 1f;

    private float maxBoxHeight = 4f;

    private bool containsOil = false;

    private bool actionTaken = false;

    public GameObject boxSpawnPoint;

    public GameObject enemySpawnPoint;

    public GameObject crate;

    //Crate generator script reference

    public GameObject oilBarrel;

    public GameObject breadstick;

    public List<GameObject> generators = new List<GameObject>();
    #endregion

    #region Properties
    //C# property. Acts as a "getter" of sorts. Returns true when count <= 0, false otherwise
    public bool IsDead => generators.Count <= 0;

    #endregion

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnDuration = new(Random.Range(minEnemySpawnTime, maxEnemySpawnTime));
        boxSpawnDuration = new(Random.Range(minBoxSpawnTime, maxBoxSpawnTime));
    }
    #endregion

    #region Main Loop
    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            //Play Death Animation once and destroy object
        }

        //Logic for when the boss is alive
        else
        {
            //Instantiate boxes at appropriate height and interval (interval function below)
            if (enemySpawnDuration.IsDone || boxSpawnDuration.IsDone)
            {
                if (actionTaken)
                {
                    //If we have taken an action, we reset timers for next action
                    CalculateRates();
                }
                else
                {
                    //Block for when we are taking an action, spawning boxes or enemies
                    if (enemySpawnDuration.IsDone)
                    {
                        //Spawn number of enemies selected within range
                        int enemyNum = Random.Range(1, 3);
                        int spawnCount = 0;
                        while (spawnCount < enemyNum)
                        {
                            Instantiate(breadstick, enemySpawnPoint.transform.position, Quaternion.identity);
                        }
                        actionTaken = true;
                        enemySpawnDuration.Reset();
                        boxSpawnDuration.Reset();
                    }
                    else if (boxSpawnDuration.IsDone)
                    {
                        //Spawn number of boxes in range
                        //Within function, calculate pick a box to be an oil barrel if bool is true
                        
                        AudioDictionary.aDict.PlayAudioClipRemote("forkLift",forkyAudioSource);
                        actionTaken = true;
                        enemySpawnDuration.Reset();
                        boxSpawnDuration.Reset();
                    }
                    
                    
                }

            }
            //Need to fix this system. If you replace true with false Unity crashes. L.
            enemySpawnDuration.IncrementUpdate(true);
            boxSpawnDuration.IncrementUpdate(true);

        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Calculates rates of probability for actions taken and objects spawned.
    /// </summary>
    void CalculateRates()
    {
        //Resetting action taken
        actionTaken = false;

        //Setting the time for both actions within the interval specified.
        enemySpawnDuration.maxTime = Random.Range(minEnemySpawnTime, maxEnemySpawnTime);
        boxSpawnDuration.maxTime = Random.Range(minBoxSpawnTime, maxBoxSpawnTime);


    }

    public void NextPhase(GameObject generator)
    {
        //Changing dependent variables for spawning logic and remove the generator
        generators.Remove(generator);
        maxBoxSpawnTime -= 1;
        minBoxSpawnTime -= 1;
        maxEnemySpawnTime -= 1;
        minEnemySpawnTime -= 1;
    }
    #endregion
}
