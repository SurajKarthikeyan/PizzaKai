using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> explosionsList;

    private static ExplosionManager exManager;

    // Start is called before the first frame update
    void Start()
    {
        exManager = this;

    }

    public static GameObject SelectExplosion(Vector2 spawnPoint)
    {
        GameObject explosionChoice = exManager.explosionsList[Random.Range(0, exManager.explosionsList.Count)];
        explosionChoice.transform.position = spawnPoint;
        return explosionChoice;
    }
}
