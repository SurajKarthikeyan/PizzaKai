using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepDish : EnemyBasic
{
    public float haltRadius = 4f;

    public GameObject bullets;
    public List<Transform> bulletPoints;
    public bool canFire;
    private bool isMoving;
    public float timeBetweenShot = 4f;
    private float shotAtTime;
    private Transform bltPointToUse;
    public bool fireRight;

    public AudioSource DeepDishShot;

    public override void Start()
    {
        base.Start();
        DeepDishShot.clip = AudioDictionary.aDict.audioDict["deepDishShot"];
    }

    public override void Update()
    {
        if (inRoom == false)
        {
            transform.position = originalPos;
            return;
        }

        base.Update();

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

    }

    override public void EnemyMovement()
    {
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
            isMoving = true;
            base.EnemyMovement();
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
        if (fireRight)
        {
            deepBullet.shotDir = new Vector3(-1, 0, 0); 
        }
        else
        {
            deepBullet.shotDir = new Vector3(1, 0, 0);
        }
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
}
