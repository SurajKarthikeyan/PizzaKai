using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private Camera realCamera;
    [SerializeField] private float grappleDistance;
    [SerializeField] private float grappleForce;
    private Vector2 hitPoint;
    private Rigidbody2D rb;
    private bool isGrappling = false;
    private float grappleMultiplier = 0f;
    [SerializeField] private LineRenderer lineRend;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //This is obviously going to work differently once its attached to the pistol alt-fire. This input grabbing is just for testing.
        if(Input.GetKey(KeyCode.Mouse1))
        {
            if(!isGrappling)
            {
                RaycastHit2D hit;
                LayerMask mask = LayerMask.GetMask("Ground", "Box");
                hit = Physics2D.Raycast(gameObject.transform.position + Vector3.up, realCamera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position, grappleDistance, mask);

                hitPoint = hit.point;

                if (hit.collider != null)
                {
                    isGrappling = true;
                }
            }
        }
        else
        {
            isGrappling = false;
        }
    }

    private void FixedUpdate()
    {
        if(isGrappling)
        {
            lineRend.enabled = true;
            rb.velocity = (rb.velocity * 0.5f) + grappleForce * (hitPoint - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized * grappleMultiplier;
            grappleMultiplier += 1f * Time.fixedDeltaTime;
            lineRend.SetPosition(0, transform.position + Vector3.up);
            lineRend.SetPosition(1, hitPoint);
        }
        else
        {
            grappleMultiplier = 0f;
            lineRend.enabled = false;
        }
    }
}
