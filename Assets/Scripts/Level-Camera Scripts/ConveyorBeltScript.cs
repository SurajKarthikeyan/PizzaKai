using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltScript : MonoBehaviour
{
    private Collider2D col2D;



    // Start is called before the first frame update
    void Start()
    {
        col2D = gameObject.GetComponent<Collider2D>();
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (col2D.ClosestPoint(new Vector2(transform.position.x, transform.position.y + 1)).y.ToString("0.00") == collision.GetContact(0).point.y.ToString("0.00"))
        {
            print("On Top");
            Rigidbody2D rigid2D = collision.gameObject.GetComponent<Rigidbody2D>();
            rigid2D.velocity = new Vector2(2, rigid2D.velocity.y);
        }
    }
    
}
