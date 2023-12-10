using UnityEngine;

public class ChickenMolotov : MonoBehaviour
{
    public GameObject firePillar;

    public Vector3 startPos;
    public float arcHeight;
    public float arcLength;

    public float duration = 1;
    public float startTime;

    private float u;

    private float p1;
    private float p2;

    private void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        u = (Time.time - startTime) / duration; 
        p1 = startPos.x + u * arcLength;
        p2 = startPos.y + arcHeight - arcHeight * ((u - 0.5f) * 2) * ((u - 0.5f) * 2);
        transform.position = new Vector3(p1, p2, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("I am in OnTriggerEnter and colliding with " + collision.gameObject.name.ToString());
        if (collision.CompareTag("Enemy")  || collision.CompareTag("EnemyProjectile")) return;

        //if (collision.gameObject.layer == 7)
        //{
        //    Debug.Log("Player collision");
        //}

        if (collision.CompareTag("Ground") || collision.CompareTag("Player") ||
            collision.CompareTag("PlayerBullet") || collision.CompareTag("PlayerFireBullet"))
        {
            Explode();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("I am in OnCollisionEnter and colliding with " + collision.gameObject.name.ToString());
    //    if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player") ||
    //        collision.collider.CompareTag("PlayerBullet") || collision.collider.CompareTag("PlayerFireBullet"))
    //    {
    //        Explode();
    //    }
    //}

    public void Explode()
    {
        Vector3 firePoint = new(transform.position.x, transform.position.y + 1.3f, 0);
        Instantiate(firePillar, firePoint, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
