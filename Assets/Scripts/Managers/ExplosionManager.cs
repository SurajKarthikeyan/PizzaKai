using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> explosionsList;

    private static ExplosionManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static GameObject SelectExplosion(Vector2 spawnPoint, float angleRotation)
    {
        GameObject explosionChoice = instance.explosionsList[Random.Range(0, instance.explosionsList.Count)];
        explosionChoice.transform.position = spawnPoint;
        explosionChoice.transform.rotation = Quaternion.Euler(0, 0, angleRotation);
        return explosionChoice;
    }
}
