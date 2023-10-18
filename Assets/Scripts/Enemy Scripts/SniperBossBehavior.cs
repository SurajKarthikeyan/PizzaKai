using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBossBehavior : EnemyBasic
{
    [SerializeField] private float smallScale = 0.75f;
    [SerializeField] private float xPosition = 5f;

    [SerializeField] private int phase = 0; //This is only serialized for testing
    private Collider2D coll;

    private bool isInBackground = true;

    public override void Start()
    {
        base.Start();
        coll = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //This must always happen before the base Update() to keep the boss from dying lol
        if(currentHP <= 0)
        {
            phase += 1;
            currentHP = maxHP;
            PhaseTransitionOut();
        }

        if(isInBackground)
        {
            transform.position = new Vector2(xPosition, player.transform.position.y + 5f);
        }

        base.Update();
    }

    public override void EnemyMovement()
    {
        //Get owned
    }

    public void PhaseTransitionOut()
    {
        //Turn off collider, move up off screen, then call a function once sufficiently off screen
        coll.enabled = false;

        rigid.velocity = Vector3.up * 5f;

        Invoke(nameof(PhaseTransitionOutReturn), 3);
    }

    private void PhaseTransitionOutReturn()
    {
        //turn smaller, let the player progress, then appear back on screen from above
        transform.localScale = Vector3.one * smallScale;

        //Remove walls here

        rigid.velocity = Vector3.down * 5f;
    }
}
