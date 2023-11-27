using UnityEngine;

public class FlameProjectile : SimpleProjectile
{
    //This variable is used for the flamethrower upgrade
    public float upRange;

    private void Awake()
    {
        range.singleValue += upRange;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Burniing Box"))
        {
            Destroy(gameObject);
        }
    }
}
