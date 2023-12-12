using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollider : MonoBehaviour
{

    private bool isKnockedBack = false;

    private Rigidbody2D player;
    [SerializeField] private GameObject shotgunObject;
    public float knockbackDuration = 1f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CapsuleCollider2D playerCollider;
    private CharacterMovementModule character;

    public int spikeDamage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        character = player.GetComponent<CharacterMovementModule>();
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.GetComponent<Character>().TakeDamage(5);
            isKnockedBack=true;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (player.gameObject.GetComponent<CharacterMovementModule>().isShotgunDashing &&(
                     col.gameObject == shotgunObject || col.gameObject == player.gameObject))
        {
            player.GetComponent<Character>().TakeDamage(spikeDamage);
            isKnockedBack = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider == playerCollider)
        {
            player.GetComponent<Character>().TakeDamage(spikeDamage);
            isKnockedBack = true;
        }
    }
}
