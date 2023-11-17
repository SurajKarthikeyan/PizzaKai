using UnityEngine;

public class ConveyorBeltScript : MonoBehaviour
{
    private Collider2D col2D;

    public float conveyorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        col2D = gameObject.GetComponent<Collider2D>();
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (col2D.ClosestPoint(new Vector2(transform.position.x, transform.position.y + 2)).y.ToString("0.00") == collision.GetContact(0).point.y.ToString("0.00"))
        {

            collision.gameObject.transform.position += Vector3.right * conveyorSpeed * Time.deltaTime;
        }
    }
    
}
