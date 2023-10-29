using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTelemDirection : MonoBehaviour
{
    public Transform rotationForTelemetry;

    Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 placeholder = gameObject.transform.position - rotationForTelemetry.position;
        gameObject.transform.localPosition = playerRb.velocity.normalized;
       
        rotationForTelemetry.transform.rotation = Quaternion.LookRotation(placeholder);
        Debug.DrawRay(rotationForTelemetry.position, placeholder );
    }
}
