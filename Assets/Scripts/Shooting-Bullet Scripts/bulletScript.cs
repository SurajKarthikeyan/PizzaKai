using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{

    private Vector3 mousePos;
    private Camera mainCam;
    public Rigidbody2D rb;
    public float force;
    public float destroyTime;
    public int damage;
    public float spread;
    public float distance;

    // Start is called before the first frame update
    virtual public void Start()
    {
        spread = spread / Vector3.Distance(this.transform.position, mousePos);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        //Gets position of mouse to figure out where it is firing and rotates the bullet in the right direction
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Get distance between bullet and mouse (& cap dist)
        distance = Vector3.Distance(this.transform.position, mousePos);
        distance -= (float) 0.5;

        if (distance > 10)
        {
            distance = 10;
        }
        // Debug.Log(distance);

        // Get % of spread we want to apply to bullet 
        distance /= 10;
        spread *= distance;

        //gets random vector3 to plug into direction and rotation
        Vector3 randomVector = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));

        //randomVector = Vector3.Distance(this.transform.position, mousePos);

        //Debug.Log(Vector3.Distance(this.transform.position, mousePos));

        //shoots based on given randomVector and mouse position at a speed equal to force. Bunch of math stuff to make sure bullets going where it should.
        Vector3 direction = mousePos - (transform.position + randomVector);
        Vector3 rotation = (transform.position + randomVector) - mousePos;

        //direction = direction + randomVector;
        //rotation = rotation + randomVector;

        //Debug.Log(direction);

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot - 180);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        //Debug.Log(mousePos);
        Destroy(this.gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Burniing Box")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Burniing Box")
        {
            Destroy(gameObject);
        }
    }
}
