using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollider : MonoBehaviour
{

    private bool isKnockedBack = false;

    private GameObject player;

    public float knockbackDuration = 1f;

    private float knockBackElapsed = 0f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack)
        {
            player.transform.position += 15 *Vector3.up * Time.deltaTime;
            knockBackElapsed += Time.deltaTime;
            if (knockBackElapsed >= knockbackDuration)
            {
                isKnockedBack = false;
                knockBackElapsed = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<Character>().TakeDamage(5);
            isKnockedBack=true;
        }
    }
}
