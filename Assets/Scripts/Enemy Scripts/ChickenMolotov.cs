using System.Collections;
using System.Collections.Generic;
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
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProjectile") return;

        if (collision.gameObject.tag == "Ground")
        {
            Vector3 flamePoint = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
            Instantiate(firePillar, flamePoint, Quaternion.Euler(0,0,0));
            Destroy(this.gameObject);
        }
    }
}
