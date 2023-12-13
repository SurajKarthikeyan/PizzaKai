using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breadstick : EnemyBasic
{
    public int size = 2;
    public float sizeScale = 0.5f;
    public int stickSplit = 2;

    //a bool to keep the cript from running death methods multiple times before actually deleting itself
    private bool actuallyDead;

    public AudioSource breadDead;

    public AudioSource breadAttack;
    public Collider2D breadCollider;
    Animator breadVanish;

    private bool hasSpawned = false;

    // Start is called before the first frame update
    override public void Start()
    {
        transform.localScale = Vector3.one * size * sizeScale;
      
        base.Start();
        breadDead.clip = AudioDictionary.aDict.audioDict["breadDead"];
        breadAttack.clip = AudioDictionary.aDict.audioDict["breadAttack"];
        breadVanish = gameObject.GetComponent<Animator>();
        deathState = false;
    }

    public override void Update()
    {
        /*if (inRoom == false)
        {
            transform.position = originalPos;
            return;
        }
        */

        if (actuallyDead)
            return;
        if (currentHP <= 0)
        {
            deathState = true;
            sRend.color = Color.white;
            if (Moving > 0)
            {
                sRend.flipX = true;
            }
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            breadDead.Play();
            breadVanish.SetBool("Death", deathState);
            if (size > 1 && !hasSpawned)
            {
                for (int i = 0; i < stickSplit; i++)
                {

                    GameObject stickGO = Instantiate<GameObject>(gameObject);
                    Breadstick stick = stickGO.GetComponent<Breadstick>();
                    stickGO.transform.position = new Vector3(transform.position.x + (i - 0), transform.position.y, transform.position.z);
                    stickGO.GetComponent<Rigidbody2D>().gravityScale = gravStore;
                    stick.sticked = true;
                    stick.originalPos = originalPos + new Vector3(i, 0, 0);
                    stick.size--;
                }
                actuallyDead = true;
            }
            else
            {
                if (!hasDropped)
                {
                    if (guaranteedDrop != null)
                    {
                        GameObject gDrop = Instantiate(guaranteedDrop);
                        gDrop.transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, 0);
                    }
                    else
                    {
                        float randomChance = Random.Range(0f, 1f);
                        if (randomChance <= dropChance)
                        {
                            GameObject hDrop = Instantiate(healthDrop);
                            hDrop.transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, 0);
                        }
                    }
                    hasDropped = true;
                }
                
                actuallyDead = true;
            }
            hasSpawned = true;
            Destroy(gameObject, 1.2f);
            breadCollider.enabled = false;
            rigid.gravityScale= 0;
            return;
            //Debug.Log("Killed breadstick and size is " + size + " and pointer " + this);
            //Destroy(this);
        }

        if (hitState && Time.time > hitTime) hitState = false;
        sRend.color = hitState ? Color.red : Color.white;

        if (jumping && Time.time > jumpTime)
        {
            rigid.gravityScale = gravStore;
            jumping = false;
            jumpTime = Time.time + jumpCooldown;
        }

        EnemyAnim.SetFloat("Movement", Moving);
        EnemyAnim.SetFloat("Attack", Fired);
        if (!facingRight && Moving > 0)
        {
            facingRight = true;
        }
        else if (facingRight && Moving < 0)
        {
            facingRight = false;
        }
        EnemyAnim.SetBool("FacingRight", facingRight);
        playerPos = player.transform.position;
        EnemyMovement();
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        breadAttack.Play();
    }


}
