using Cinemachine;
using UnityEngine;

public class AssignPlayerFollow : MonoBehaviour
{
    private void Awake()
    {
        this.RequireComponent(out CinemachineVirtualCamera vCam);
        vCam.Follow = GameObject.FindWithTag("PlayerFollow").transform;
    }
}