using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentWander : MonoBehaviour {
    NavMeshAgent agent;

    private float waitUntil = -1;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        if ( !agent.isOnNavMesh ) return;

        if ( waitUntil > -1 ) {
            if ( Time.time >= waitUntil ) {
                waitUntil = -1;
                agent.isStopped = false;
            }
        } else {
            if ( agent.remainingDistance < 0.1f ) {
                Vector3 destination = Random.insideUnitSphere * 15;
                destination.y = 0;
                agent.SetDestination( destination );
                XnTelemetry.Telemetry_Cloud_Multiplayer.LOG( "Turn", transform );
                //Debug.Log( $"{gameObject.name} > {destination}" );
            } else {
                if ( Random.value <= 0.0025f ) {
                    waitUntil = Time.time + 0.5f;
                    agent.isStopped = true;
                    XnTelemetry.Telemetry_Cloud_Multiplayer.LOG( "Stop", transform );
                }
            }
        }
    }
}
