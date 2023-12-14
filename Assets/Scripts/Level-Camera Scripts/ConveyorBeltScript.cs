using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorBeltScript : MonoBehaviour
{
    private Collider2D col2D;

    public float conveyorSpeed;

    public float conveyorAnimationSpeed;

    public Tilemap conveyorTilemap;

    // Start is called before the first frame update
    void Start()
    {
        col2D = gameObject.GetComponent<Collider2D>();
        conveyorTilemap.animationFrameRate = 1;
        conveyorSpeed = -1f;
    }

    private void Update()
    {
        conveyorAnimationSpeed = conveyorTilemap.animationFrameRate;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (col2D.ClosestPoint(new Vector2(transform.position.x, transform.position.y + 2)).y.ToString("0.00") == collision.GetContact(0).point.y.ToString("0.00"))
        {

            collision.gameObject.transform.position += Vector3.right * conveyorSpeed * Time.deltaTime;
        }
    }
    
    public void SetConveyorSpeed(float speed)
    {
        if (speed != 0)
        {
            conveyorTilemap.animationFrameRate *= Mathf.Abs(speed);
        }
        else
        {
            conveyorTilemap.animationFrameRate = speed;
        }
        conveyorSpeed = speed;
    }
}
