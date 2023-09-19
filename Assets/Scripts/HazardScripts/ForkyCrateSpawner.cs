using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkyCrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cratePrefabs;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject oilCrate;

    public void SpawnCrate(int numCrates = 1, bool hasOil = false)
    {
        GameObject crateGO = Instantiate<GameObject>(cratePrefabs[numCrates]);
        crateGO.transform.position = spawnPos.position;

        if(hasOil)
        {
            GameObject randCrate = crateGO.transform.GetChild(Random.Range(1, numCrates + 1)).gameObject;
            Vector3 tempPos = new Vector3(randCrate.transform.position.x, randCrate.transform.position.y);
            Destroy(randCrate);
            GameObject oilCrateGO = Instantiate<GameObject>(oilCrate);
            oilCrateGO.transform.position = randCrate.transform.position;
            oilCrateGO.transform.SetParent(crateGO.transform);
            
        }
    }
}
