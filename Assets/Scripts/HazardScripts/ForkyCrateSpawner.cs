using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkyCrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cratePrefabs;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private GameObject oilCrate;

    public void SpawnCrate(int numCrates = 1, bool hasOil = false)
    {
        GameObject crateGO = Instantiate<GameObject>(cratePrefabs[numCrates]);
        crateGO.transform.position = spawnPos;

        if(hasOil)
        {
            GameObject randCrate = crateGO.transform.GetChild(Random.Range(0, numCrates + 1)).gameObject;
            GameObject oilCrateGO = Instantiate<GameObject>(oilCrate);
            oilCrateGO.transform.position = randCrate.transform.position;
            Destroy(randCrate);
        }
    }
}
