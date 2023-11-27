using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CM_ConnectToPlayer : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    public PlayerControlModule pcm;

    private void Reset()
    {
        SetVars();
    }

    private void Awake()
    {
        SetVars();
    }

    private void SetVars()
    {
        this.RequireComponent(out vCam);

        GameObject.FindGameObjectWithTag("Player")
            .RequireComponent(out pcm);
    }
}