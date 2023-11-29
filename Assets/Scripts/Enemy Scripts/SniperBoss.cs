using UnityEngine;

public class SniperBoss : MonoBehaviour
{
    [Tooltip("Time after a snipe is finished for a new snipe to start.")]
    [SerializeField] private float snipeCooldown = 3f;
    [Tooltip("Time it takes from the start to the finish of a snipe.")]
    [SerializeField] private float snipeTime = 3f;
    [Tooltip("Time after a bullet storm attack is finished for a new one to start.")]
    [SerializeField] private float bulletStormCooldown = 3f;
    [Tooltip("Time that a bullet storm lasts.")]
    [SerializeField] private float bulletStormTime = 3f;

    [SerializeField] private GameObject snipePrefab;
    [SerializeField] private GameObject player;

    private bool canSnipe = false;
    private bool canBulletStorm = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(ResetSnipe), snipeCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if(canSnipe)
        {
            Snipe();
        }
    }

    private void Snipe()
    {
        canSnipe = false;

        GameObject snipeGO = Instantiate<GameObject>(snipePrefab);
        SnipeObject snipeObject = snipeGO.GetComponent<SnipeObject>();

        snipeObject.StartSnipe(snipeTime, player);

        Invoke(nameof(ResetSnipe), snipeTime + snipeCooldown);
    }

    private void ResetSnipe()
    {
        canSnipe = true;
    }

    private void ResetBulletStorm()
    {
        canBulletStorm = true;
    }
}
