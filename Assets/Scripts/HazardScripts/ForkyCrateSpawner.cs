using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkyCrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cratePrefabs;
    [SerializeField] private Vector3 spawnPos;

    public void SpawnCrate(int numCrates = 1)
    {
        GameObject crateGO = Instantiate<GameObject>(cratePrefabs[numCrates]);
        crateGO.transform.position = spawnPos;
    }
}
