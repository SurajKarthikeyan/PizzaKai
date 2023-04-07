using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public RespawnScript respawn;
    public Collider2D coll;

    public Animator Checkpoint;
    public bool Activated;
    public bool ActiveIdle;
    public float time;

    void Awake()
    {
        //respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
        coll = GetComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Checkpoint.SetBool("Activated", Activated);
        Checkpoint.SetBool("ActiveIdle", ActiveIdle);

        if (coll.enabled == false)
        {
            StartCoroutine(Idle());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Activated = true;
            respawn.respawnPoint = this.gameObject;
            coll.enabled = false;
        }
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSecondsRealtime(2);
        Activated = false;
        ActiveIdle = true;
    }
}
