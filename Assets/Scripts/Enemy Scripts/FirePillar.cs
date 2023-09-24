using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillar : MonoBehaviour
{
    private float startTime;
    public float flameDur = 2;
    public bool Flame;

    public Animator EnemyFlame;
    public AudioSource flameAudio;

    private void Awake()
    {
        flameAudio = GameObject.Find("FlameAudio").GetComponent<AudioSource>();
        EnemyFlame.SetBool("Flame", Flame);
        Flame = true;
        flameAudio.Play();
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= startTime + flameDur) Destroy(this.gameObject);

        //if (Time.time <= startTime + flameDur) Flame = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(5);
        }
    }
}
