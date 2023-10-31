using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    public static CinemachineCameraShake instance;
    [SerializeField]
    CinemachineVirtualCamera cineCamera;

    private float duration;
    public float durationTotal;

    public float intensity;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin cmBasicPerlin;

    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
        //cmBasicPerlin = cineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cineCamera.Name != CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Name)
        {
            cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
            cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
        }
        if (duration > 0)
        {
            duration -= Time.deltaTime;
         
            cmBasicPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, duration / durationTotal);
        }
        else
            cmBasicPerlin.m_AmplitudeGain = 0f;
    }

    public void ShakeScreen()
    {
        cmBasicPerlin.m_AmplitudeGain = intensity;

        duration = durationTotal;
    }
}
