using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollider : MonoBehaviour
{

    private bool isKnockedBack = false;

    private Rigidbody2D player;

    public float knockbackDuration = 1f;

    private float knockBackElapsed = 0f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack)
        {
            player.velocity = Vector2.up * 15f;
            isKnockedBack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.GetComponent<Character>().TakeDamage(5);
            isKnockedBack=true;
        }
    }
}
