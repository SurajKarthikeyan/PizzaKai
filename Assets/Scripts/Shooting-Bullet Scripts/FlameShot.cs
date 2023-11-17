using UnityEngine;

public class FlameShot : bulletScript
{
    public GameObject[] SceneBullets;
    // Start is called before the first frame update
    override public void Start()
    {
        SceneBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");

        foreach (GameObject bullets in SceneBullets)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), bullets.GetComponent<Collider2D>());
        }

        
        base.Start();
        rb.drag = 5;

    }
    // Update is called once per frame
    override public void Update()
    {
        
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }


        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Burniing Box")
        {
            Destroy(gameObject);
        }
    }

}
