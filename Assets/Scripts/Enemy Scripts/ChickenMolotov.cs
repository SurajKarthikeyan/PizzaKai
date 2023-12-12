using UnityEngine;

public class ChickenMolotov : MonoBehaviour
{
    public GameObject firePillar;

    public GameObject explosion;

    public Vector3 startPos;
    public float arcHeight;
    public float arcLength;

    public float duration = 1;
    public float startTime;

    private float u;

    private float p1;
    private float p2;

    private Collider2D shootcollider;

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

    public void Explode(bool onGround)
    {
        GameObject fireType;
        float offset = 0;
        if (onGround)
        {
            fireType = firePillar;
            offset = 1.5f;
        }
        else
        {
            fireType = explosion;
        }
        Vector3 firePoint = new(transform.position.x, transform.position.y + offset, 0);
        Instantiate(fireType, firePoint, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
