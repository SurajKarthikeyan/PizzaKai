using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> objectsSpawned = new();
    public Range spawnNum = new(1);
    private int numSpawned;

    [Tooltip("Spawn objects after how many seconds?")]
    public float spawnTime = 0.1f;

    [Tooltip("Spawn radius")]
    public float spawnRadius;

    public float maxStartForce;

    public float startRotation;
    public float rotationDeviation;

    public void Start()
    {
        Invoke(nameof(StartSpawn), 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    /// <summary>
    /// For some reason, the entities fail to spawn in the correct location
    /// if the spawning is not deferred.
    /// </summary>
    private void StartSpawn()
    {
        if (spawnTime > 0)
        {
            StartCoroutine(SpawnObjectsCoroutine());
        }
        else
        {
            for (int i = 0; i < spawnNum.Select(); i++)
            {
                SpawnObjects();
            }
        }
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        SpawnObjects();

        numSpawned++;

        yield return new WaitForSeconds(spawnTime);

        if (numSpawned < spawnNum.Select())
            StartCoroutine(SpawnObjectsCoroutine());
    }

    private void SpawnObjects()
    {
        foreach (var toSpawn in objectsToSpawn)
        {
            TransformSet(Instantiate(toSpawn));
        }
    }

    private void TransformSet(GameObject enemyObj)
    {
        enemyObj.SetActive(true);

        Vector3 offset = new(0, 0, RNGExt.RandomFloat(0, spawnRadius));
        offset = Quaternion.Euler(0, RNGExt.RandomFloat(0, 360), 0) * offset;

        if (enemyObj.HasComponent(out NavMeshAgent nma))
        {
            nma.Warp(transform.position + offset);
            enemyObj.transform.rotation =
                Quaternion.Euler(0, startRotation +
                RNGExt.RandomFloat(
                    -rotationDeviation, rotationDeviation), 0
                    );
        }
        else
        {
            enemyObj.transform.SetPositionAndRotation(
                transform.position + offset,
                Quaternion.Euler(0, startRotation +
                RNGExt.RandomFloat(
                    -rotationDeviation, rotationDeviation), 0
                    )
            );
        }

        if (enemyObj.HasComponent(out Rigidbody2D r2d))
        {
            r2d.AddForce(RNGExt.WithinCircle(Mathf.Abs(maxStartForce)));
        }

        objectsSpawned.Add(enemyObj);

        /// Uncomment for spawn debug
        //print($"Spawn at {enemyObj.transform.position} from spawner at {transform.position}");
    }

    private void OnDestroy()
    {
        foreach (var obj in objectsSpawned)
        {
            Destroy(obj);
        }
    }
}
