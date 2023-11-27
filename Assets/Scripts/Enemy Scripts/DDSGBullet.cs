using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDSGBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float destroyTime = 0.3f;
    public int damage = 5;
    public float force = 1;
    public float spread = 1;
    public Vector3 shotDir;
    private bool firstProj = true;

    void Start()
    {
        CheckBullets();
        CloneBullet();

        rb = GetComponent<Rigidbody2D>();

        // Random vector for directional spread
        Vector3 randomVector = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread),0);

        Vector3 direction = shotDir - (transform.position + randomVector);
        direction = new Vector3(direction.x * shotDir.x, direction.y, 0);
        Vector3 rotation = (transform.position + randomVector);

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void Update()
    {
        Destroy(this.gameObject, destroyTime);
    }

    void CloneBullet()
    {
        if (firstProj)
        {
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
            Instantiate(this.gameObject);
        }
    }

    void CheckBullets()
    {
        DDSGBullet[] findList = FindObjectsOfType<DDSGBullet>();


        for (int i = 0; i < findList.Length; i++)
        {
            if (findList.Length > 1)
            {
                //if the list is more than 1, turn off firstProjectile
                findList[i].firstProj = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Character>().TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
