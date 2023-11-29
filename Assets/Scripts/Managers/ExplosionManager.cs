using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to a(n) 
    /// <see cref="ExplosionManager"/> instance.
    /// </summary>
    private static ExplosionManager instance;
    
    /// <inheritdoc cref="instance"/>
    public static ExplosionManager Instance => instance;
    #endregion
    
    private void Awake()
    {
        if (instance == null)
        this.InstantiateSingleton(ref instance);
    }

    [SerializeField] private List<GameObject> explosionsList;
 

    /// <summary>
    /// Spawns a random explosion.
    /// </summary>
    /// <param name="spawnPoint">Where to spawn the explosion.</param>
    /// <param name="angleRotation">Y rotation of the explosion.</param>
    /// <returns>The instantiated explosion.</returns>
    public GameObject SelectExplosionRandom(Vector2 spawnPoint,
        float angleRotation)
    {
        GameObject explosionChoice = Instantiate(
            explosionsList.RandomSelectOne()
        );
        explosionChoice.transform.SetPositionAndRotation(
            spawnPoint,
            Quaternion.Euler(0, 0, angleRotation)
        );

        return explosionChoice;
    }

    public void MakeSparks(RaycastHit2D bulletHit, Transform bulletTransform,GameObject bulletSpark)
    {
        Vector2 bulletPos = new Vector2(bulletTransform.position.x, bulletTransform.position.y);
        var spark = Instantiate(bulletSpark);
        spark.transform.MatchOther(bulletTransform);

        spark.transform.right = Vector3.Reflect(bulletHit.point - bulletPos, bulletHit.normal.ToVector3());
        

        StartCoroutine(DeleteSparks(spark));
    }

    IEnumerator DeleteSparks(GameObject pSystem)
    {
        yield return new WaitForSeconds(3);
        Destroy(pSystem);
    }
}
