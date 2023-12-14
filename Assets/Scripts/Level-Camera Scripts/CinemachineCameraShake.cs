using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CinemachineCameraShake : MonoBehaviour
{
    public static CinemachineCameraShake instance;
    [SerializeField]
    CinemachineVirtualCamera cineCamera;

    private Transform cameraTransform;

    private float duration;
    public float durationTotal;
    private float currentTotalDuration;

    public float intensity;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin cmBasicPerlin;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (cineCamera == null)
            {
                cineCamera = FindObjectOfType<CinemachineVirtualCamera>();
                if (cineCamera != null)
                {
                    cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
                    cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);

                    cameraTransform = Camera.main.gameObject.transform;
                }
            }
            else
            {
                cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
                cameraTransform = Camera.main.gameObject.transform;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (cameraTransform == null)
                cameraTransform = Camera.main.gameObject.transform;

            if (CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera == null)
                return;
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
                    cmBasicPerlin.m_AmplitudeGain /= 2;
                }
                else
                {
                    cmBasicPerlin.m_AmplitudeGain = 0f;
                    cameraTransform.eulerAngles = Vector3.zero;
                }

            }
            else
            {
                if (CinemachineCore.Instance.GetActiveBrain(0))
                {
                    cineCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
                    cineCamera.RequireComponentInChildren(out cmBasicPerlin, "CinemachineBasicMultiChannelPerlin", true, false);
                }
            }
        }   
    }

    public void ShakeScreen()
    {
        cmBasicPerlin.m_AmplitudeGain = intensity/2;

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
