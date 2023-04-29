using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //BASIC MOVEMENT VARIABLES
    private float moveSpeed = 10f;
    public float horizontalMove = 0f;
    public bool facingRight = true;
    public bool isAlting = false;
    

    //DODGE VARIABLES
    private bool canDodge;
    private bool isDodging;
    private float dodgeForce = 20f;
    private float dodgeTime = 0.2f;
    private float dodgeCoolDown = 2f;
    private bool invulnerable;

    //JUMP VARIABLES

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpForce = 12f;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float originalGravity;

    // Knockback Variables
    private Vector3 hitLoc;
    public float kbSpeed = 0.75f;
    public float kbDur = 1;
    public bool isKBed;
    

    //Animator Stuff

    public Animator animator;
    public bool jumpingNow;

    public float incomingDamage;

    public Flip flip;
    public UICode UIScript;

    public SpriteRenderer spriteRender;

    public Rigidbody2D rigid;
    public bool level1Key = false;
    public bool level2Key = false;

    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask jumpLayer;
    [SerializeField] private LayerMask platformLayer;

    // Start is called before the first frame update

    void Start()
    {
        //tells the code the player isn't dodging and can dodge on start
        isDodging = false;

        canDodge = true;

        invulnerable = false;
        
        rigid = GetComponent<Rigidbody2D>();

        originalGravity = rigid.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            //Flashes player on damage hit
            if(isKBed) StartCoroutine(DamageFlash());
            if (!isKBed)
            {
                //spriteRender.color = invulnerable ? Color.yellow : Color.white;
            }

            //stops player from moving while dodging
            if (isDodging)
            {
                return;
            }

            //stops player from moving while knocking back
            if (isKBed)
            {
                rigid.velocity = Vector2.zero;
                Vector3 delta = transform.position - hitLoc;
                float velX = (delta.x >= 0) ? 1 : -1;
                float velY = (delta.y >= 0) ? 1 : -1;
                // float gravStore = rigid.gravityScale;
                // rigid.gravityScale = 0;
                rigid.velocity = new Vector2(velX, velY) * kbSpeed;

                return;
            }

            //Gets horizontal input 
            horizontalMove = Input.GetAxisRaw("Horizontal");

            if (isJumpable())
            {

                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            //if the jump button (space) is clicked and the player is grounded That way no double jumps and no jumping while falling.
            //Can easily change this to allow for double jump in future.
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {

                //jumps based off jumpForce
                rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);

                jumpBufferCounter = 0f;


            }

            //allows for longer hold to make jump high and vice versa
            if ((Input.GetButtonUp("Jump") && rigid.velocity.y > 0f) || (rigid.velocity.y < 0.1f && rigid.velocity.y > -0.1f && coyoteTimeCounter < 0f))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
                rigid.gravityScale = 6;
                coyoteTimeCounter = 0f;
            }

            if (Input.GetKeyDown("left shift") && canDodge)
            {
                StartCoroutine(Dodge());
            }

            //If the player is on the ground and isn't on layer 7, it sets the player on layer 7
            if (IsGrounded() == true && gameObject.layer != 7 && isDodging == false)
            {
                //player layer
                gameObject.layer = 7;

            }

            if (IsGrounded() == true)
            {

                jumpingNow = false;
                rigid.gravityScale = originalGravity;
            }

            if (isPlatformed() == true)
            {

                jumpingNow = false;
                rigid.gravityScale = originalGravity;
            }

            if (isPlatformed() == false && IsGrounded() == false)
            {
                jumpingNow = true;

            }

            //If the player hits S and the player is on the One Way, they fall through since both are on layer 6
            if (isPlatformed() == true && Input.GetKey(KeyCode.S))
            {
                StartCoroutine(Phase());


            }

            animator.SetFloat("RightLeftMovement", Mathf.Abs(horizontalMove));
            animator.SetBool("Jumping", jumpingNow);
        }
        

    }

    private void FixedUpdate()
    {
        if (!UIScript.isPaused && !UIScript.isDying)
        {
            if (isDodging || isAlting || isKBed)
            {
                //do nothing
            }
            else
            {
                //this is what makes the player move. When it is commented out, shotgun script works better, but not quite up to snuff
                rigid.velocity = new Vector2(horizontalMove * moveSpeed, rigid.velocity.y);
            }
        }

    }

    //checks if player is on the ground
    private bool IsGrounded()
    {
        //changed this to 0.1 from 0.2 to maybe fix the phase down bug
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    //Checks if the player is on the jump layer aka on any hard surface
    private bool isJumpable()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, jumpLayer);
    }

    //checks if player is on a platform (specifically one that can be dropped through)
    private bool isPlatformed()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
    }

    public IEnumerator Dodge()
    {
        //since this gets called when the player is dodging, canDodge is now false and isDodging is now true
        canDodge = false;
        isDodging = true;
        invulnerable = true;

        //enemy layer
        gameObject.layer = 10;
        StartCoroutine(UIScript.dashFill());
        //saves gravity value before we change it
        
        //makes it so the player does not fall during dodge
        rigid.gravityScale = 0f;
        //dodge in the direction the character is facing
        if (Input.GetKey(KeyCode.D))
        {
            rigid.velocity = new Vector2(dodgeForce, 0f);
        }
        else 
        {
            rigid.velocity = new Vector2(-dodgeForce, 0f);
        }

        //there is no trail renderer attached right now, but if one is attached it will emit while dodging
        trail.emitting = true;
        
        yield return new WaitForSeconds(dodgeTime);
        //turns trail off when dodging stops
        trail.emitting = false;
        //sets gravity back to OG setting
        rigid.gravityScale = originalGravity;
        isDodging = false;
        invulnerable = false;
        //player layer
        gameObject.layer = 7;
        yield return new WaitForSeconds(dodgeCoolDown);
        canDodge = true;
    }

    //coroutine that gets called above. Sets player to platform layer for half a second so they can fall
    //through a platform but still land on a platform below 
    public IEnumerator Phase()
    {
        //oneWayPlatform layer
        gameObject.layer = 6;
        yield return new WaitForSeconds(0.25f);
        //player layer
        gameObject.layer = 7;

    }

    public IEnumerator altCool()
    {
        yield return new WaitForSeconds(2f);
        isAlting = false;
    }

    public IEnumerator damageCool()
    {
        invulnerable = true;
        yield return new WaitForSeconds(2f);
        invulnerable = false;
    }

    public IEnumerator Knockback()
    {
        yield return (new WaitForSeconds(kbDur));
        // rigid.gravityScale = gravStore;
        rigid.velocity = Vector2.zero;
        isKBed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && invulnerable == false)
        {
            hitLoc = collision.transform.position;
            isKBed = true;
            rigid.velocity = Vector2.zero;
            StartCoroutine(Knockback());
            if (collision.gameObject.GetComponent<EnemyBasic>() != null)
            {
                incomingDamage = collision.gameObject.GetComponent<EnemyBasic>().enemyDamage;
            }
            else
            {
                incomingDamage = 0.25f;
            }
            
            
            UIScript.LoseHealth(incomingDamage);
            StartCoroutine(damageCool());
            
            //Debug.Log("Damage taken");
        }
        
        isAlting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile" && invulnerable == false)
        {
            incomingDamage = 0.25f;
            UIScript.LoseHealth(incomingDamage);
            StartCoroutine(damageCool());
        }
        
    }

    private IEnumerator DamageFlash()
    {
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(.1f);
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(.1f);
    }

}
