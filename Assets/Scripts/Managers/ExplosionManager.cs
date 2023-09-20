using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> explosionsList;

    public static ExplosionManager explosionManager;

    // Start is called before the first frame update
    void Start()
    {
        explosionManager = this;
    }

    public GameObject SelectExplosionRandom(Vector2 spawnPoint, float angleRotation)
    {
        GameObject explosionChoice = explosionManager.explosionsList[Random.Range(0, explosionManager.explosionsList.Count)];
        explosionChoice.transform.position = spawnPoint;
        explosionChoice.transform.rotation = Quaternion.Euler(0, 0, angleRotation);
        return explosionChoice;
    }
}
