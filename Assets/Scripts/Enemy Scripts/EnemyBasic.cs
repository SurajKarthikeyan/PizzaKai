using UnityEngine;

public abstract class EnemyBasic : MonoBehaviour
{
    #region Enums
    public enum EnemyState
    {
        NotActive,
        Active,
        Hit,
        Dead
    }

    #endregion

    #region Variables
    // Health and Death
    public int currentHP;
    public int maxHP;
    protected bool hitState;
    protected float hitDur = 0.25f;
    protected float hitTime;
    public bool deathState;

    // Specifically handles breadstick stuff, but needed to reference back here
    protected bool sticked;

    // Player Detection Radius, Movement Speed and Damage to Deal
    public float detRadius = 10f;
    public float speed = 2;
    public float enemyDamage = 0.25f;
    public bool inRoom = false;
    public GameObject boundChild;
    protected Vector3 originalPos;

    // Jumping
    protected bool jumping = false;
    public float jumpDuration = 0.5f;
    protected float jumpTime;
    protected float gravStore;
    public float jumpSpeed = 1;
    public float jumpCooldown = 2;

    // Dropping Pickups
    public GameObject guaranteedDrop;
    public GameObject healthDrop;
    [Range(0, 1)]
    public float dropChance;

    // Animations
    public Animator EnemyAnim;
    public float Moving;
    public float Fired;
    public float Idle;

    // Enemy's Position, rigid and sRend
    protected Vector3 enemyPos;
    protected Rigidbody2D r2d;
    protected SpriteRenderer sRend;

    // References to the Player
    protected GameObject player;
    protected Vector3 playerPos;
    #endregion

    // Start is called before the first frame update
    virtual public void Start()
    {
        currentHP = maxHP;
        deathState = false;

        enemyPos = transform.position;
        if (!sticked) originalPos = enemyPos;
        r2d = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
        gravStore = r2d.gravityScale;

        player = GameObject.Find("Player");
        playerPos = player.transform.position;

    }

    // Update is called once per frame
    virtual public void Update()
    {
        // Handles Death and Drops
        if (currentHP <= 0)
        {
            deathState = true;

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
            //LayersManager.Instance
            Destroy(this.gameObject);
        }

        // Handles Flashing on Hit
        if (hitState && Time.time > hitTime) hitState = false;
        sRend.color = hitState ? Color.red : Color.white;

        // Handles JumpDuration and Resetting Jump
        if (jumping && Time.time > jumpTime)
        {
            r2d.gravityScale = gravStore;
            jumping = false;
            jumpTime = Time.time + jumpCooldown;
        }

        // Animations and playerRef, calling Movement
        EnemyAnim.SetFloat("Movement", Moving);
        EnemyAnim.SetFloat("Attack", Fired);
        EnemyAnim.SetFloat("Idle", Idle);
        playerPos = player.transform.position;
        EnemyMovement();

    }

    // Basic Enemy Movement, used by Breadstick and DeepDish
    virtual public void EnemyMovement()
    {
        // Jumping Priority
        if (jumping) return;
        if ((playerPos.x >= enemyPos.x - 0.25f) && (playerPos.x <= enemyPos.x + 0.25f) &&
            (playerPos.y > enemyPos.y) && !jumping && (Time.time > jumpTime))
        {
            jumping = true;
            jumpTime = Time.time + jumpDuration;
            Invoke("EnemyJump", 0.15f);
        }
        // If Jumping conditions not met, move normally
        else if ((playerPos.x >= enemyPos.x - detRadius) && (playerPos.x <= detRadius + enemyPos.x))
        {
            if (playerPos.y > enemyPos.y + detRadius / 2 || playerPos.y < enemyPos.y - detRadius / 2) return;

            if (playerPos.x >= enemyPos.x - detRadius && playerPos.x <= enemyPos.x)
            {
                //Debug.Log("Player in from left");
                r2d.velocity = Vector2.left * speed;
                enemyPos = transform.position;
                Moving = -1;
                Idle = 0;
            }
            if (playerPos.x <= enemyPos.x + detRadius && playerPos.x >= enemyPos.x)
            {
                //Debug.Log("Player in from right");
                r2d.velocity = Vector2.right * speed;
                enemyPos = transform.position;
                Moving = 1;
                Idle = 0;
            }
        }
        // Otherwise, stay idle
        else if ((playerPos.x <= enemyPos.x - detRadius) || (playerPos.x >= detRadius + enemyPos.x))
        {
            r2d.velocity = new Vector2(0, 0);
            enemyPos = transform.position;
            Moving = 0;
        }
    }

    virtual public void EnemyJump()
    {
        r2d.velocity = new Vector2(0, jumpSpeed);
        r2d.gravityScale = 0;
    }

    // Makes sure can only be hit by player bullets, deals damage
    virtual public void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Was hit");
        if (collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "PlayerFireBullet")
        {
            currentHP = currentHP - (int)collision.gameObject.GetComponent<bulletScript>().damage;
            Destroy(collision.gameObject);

            hitState = true;
            hitTime = Time.time + hitDur;
        }
    }

    // Handles Enemy being inRoom or not for detection purposes.
    virtual public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            boundChild = other.gameObject.GetComponent<BoundaryManager>().boundary;
            if (boundChild.activeInHierarchy == false)
            {
                inRoom = false;
            }
            else { inRoom = true; }
        }

    }


}
