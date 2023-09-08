using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillar : MonoBehaviour
{
    private float startTime;
    public float flameDur = 2;
    public bool Flame;

    public Animator EnemyFlame;

    private void Awake()
    {
        EnemyFlame.SetBool("Flame", Flame);
        Flame = true;
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= startTime + flameDur) Destroy(this.gameObject);

        //if (Time.time <= startTime + flameDur) Flame = true;
    }
}
