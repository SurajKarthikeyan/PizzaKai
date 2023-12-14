using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepDish : EnemyBasic
{
    public float haltRadius = 4f;
    public float patrolDist = 2f;
    private float startPosX;
    private Vector2 rememberPatrolDir;

    public GameObject bullets;
    public List<Transform> bulletPoints;
    public bool canFire;
    private bool isMoving;
    public float timeBetweenShot = 4f;
    private float shotAtTime;
    private Transform bltPointToUse;
    public bool fireRight;
    public bool hasDied = false;

    public AudioSource DeepDishShot;

    public override void Start()
    {
        base.Start();
        facingRight = true;
        DeepDishShot.clip = AudioDictionary.aDict.audioDict["deepDishShot"];

        startPosX = transform.position.x;
        rememberPatrolDir = Vector2.left * speed;
        Idle = 0;
        Moving = -1;

    }

    public override void Update()
    {
        /*if (inRoom == false)
        {
            transform.position = originalPos;
            return;
        }
        */

        base.Update();
        if (deathState && !hasDied)
        {
            hasDied = true;
            sRend.color = Color.white;
            this.gameObject.GetComponent<Animator>().SetBool("Dead", true);
            Destroy(this.gameObject.GetComponent<Collider2D>());
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke("DDDeath", 1f);
        }

        else
        {
            if (!isMoving && (Time.time >= shotAtTime + timeBetweenShot)) canFire = true;

            if (canFire == true && fireRight)
            {
                Fired = 1;
                DeepDishShot.Play();
            }
            else if (canFire == true && !fireRight)
            {
                Fired = -1;
                DeepDishShot.Play();
            }
            ShootShot();
            //Debug.DrawRay(bltPointToUse.position,  playerPos - bltPointToUse.position);
        }

    }


    public void DDDeath()
    {
        Destroy(this.gameObject);
    }
    override public void EnemyMovement()
    {
        //if player inside haltRadius
        if ((playerPos.x >= enemyPos.x - haltRadius) && (playerPos.x <= haltRadius + enemyPos.x))
        {
            if (playerPos.y > enemyPos.y + detRadius/2 || playerPos.y < enemyPos.y - detRadius/2) return;

            if (playerPos.x >= enemyPos.x - haltRadius && playerPos.x <= enemyPos.x)
            {
                //Debug.Log("Player in from left");
                fireRight = false;
                Idle = -1;
                Moving = 0;
                rigid.velocity = Vector2.zero;
                enemyPos = transform.position;
                bltPointToUse = bulletPoints[0];
            }
            if (playerPos.x <= enemyPos.x + haltRadius && playerPos.x >= enemyPos.x)
            {
                //Debug.Log("Player in from right");
                fireRight = true;
                Idle = 1;
                Moving = 0;
                rigid.velocity = Vector2.zero;
                enemyPos = transform.position;
                bltPointToUse = bulletPoints[1];
            }
            isMoving = false;
        }
        else
        {
            Idle = 0;
            SwitchPatrolPoint();
            
            enemyPos = transform.position;
            isMoving = true;
        }
    }

    // Checks if the enemy can shoot, then shoots the shotgun. 
    public void ShootShot()
    {
        if (!canFire)
        {
            Fired = 0;
            return;
            
        }

        GameObject bulletGO = Instantiate(bullets, bltPointToUse.position, Quaternion.identity);

        if (bulletGO.GetComponent<DDSGBullet>() == null)
        {
            Debug.LogError("Could not find DDSGBullet Script");
        }
        DDSGBullet deepBullet = bulletGO.GetComponent<DDSGBullet>();
        
        deepBullet.shotDir = player.GetComponent<Collider2D>().bounds.center - bltPointToUse.position;
        
        
        canFire = false;
        
        shotAtTime = Time.time;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            transform.position = originalPos;
        }
    }

    public void SwitchPatrolPoint()
    {
        if (rigid.velocity.x >= 0 && transform.position.x >= startPosX + patrolDist)
        {
            rigid.velocity = Vector2.left * speed;
            rememberPatrolDir = rigid.velocity;
            Moving = -1;
        }
        else if (rigid.velocity.x <= 0 && transform.position.x <= startPosX - patrolDist)
        {
            rigid.velocity = Vector2.right * speed;
            rememberPatrolDir = rigid.velocity;
            Moving = 1;
        }
        //return to partol after stopping
        else if (!isMoving)
        { 
            rigid.velocity = rememberPatrolDir;
            if (rememberPatrolDir.x > 0)
                Moving = 1;
            else
                Moving = -1;
        }


        //if stuck not moving, turn around
        else if (rigid.velocity == Vector2.zero)
        {
            rigid.velocity = -rememberPatrolDir;
            rememberPatrolDir = rigid.velocity;
            Moving = -Moving;
        }

        else
            rigid.velocity = rememberPatrolDir;
            
    }
}
