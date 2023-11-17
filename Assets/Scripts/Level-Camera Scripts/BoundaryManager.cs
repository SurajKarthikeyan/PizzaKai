using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    private BoxCollider2D managerBox; //BoxCollider of the BoundaryManager
    private Transform player; //player's position
    public GameObject boundary; //The camera's boundary that will be activated and deactivated
    // Start is called before the first frame update
    void Start()
    {
        managerBox = GetComponent<BoxCollider2D>();
        //gets the player's position
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageBoundary();
    }

    void ManageBoundary()
    {
        //if the player is inside the boundary, set active
        if (managerBox.bounds.min.x < player.position.x && player.position.x < managerBox.bounds.max.x &&
            managerBox.bounds.min.y < player.position.y && player.position.y < managerBox.bounds.max.y)
        {
            boundary.SetActive(true);
        } else
        {
            boundary.SetActive(false);
        }
    }
}
