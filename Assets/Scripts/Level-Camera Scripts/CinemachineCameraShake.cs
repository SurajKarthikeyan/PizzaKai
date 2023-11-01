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
    private float currentTotalDuration;

    public float intensity;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin cmBasicPerlin;

    private void Awake()
    {
        if (instance == null)
        this.InstantiateSingleton(ref instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cineCamera == null)
        {
            cineCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (cineCamera != null)
            {
                cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
                cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cineCamera != null)
        {
            if (cineCamera.Name != CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Name)
            {
                cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
                cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
            }

            if (duration > 0)
            {
                duration -= Time.deltaTime;

                cmBasicPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, duration / currentTotalDuration);
            }
            else
            {
                cmBasicPerlin.m_AmplitudeGain = 0f;
            }
                
        }
        else
        {
            cineCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (cineCamera != null)
            {
                cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
                cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
            }
        }
    }

    public void ShakeScreen()
    {
        cmBasicPerlin.m_AmplitudeGain = intensity;

        duration = durationTotal;
        currentTotalDuration = durationTotal;
    }

    public void ShakeScreen(float _intensity, float _duration)
    {
        cmBasicPerlin.m_AmplitudeGain = _intensity;

        duration = _duration;
        currentTotalDuration = _duration;
    }
}
