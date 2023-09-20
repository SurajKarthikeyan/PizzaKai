using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public bool bossStart = false;

    private bool lerpCam = false;

    public GameObject mainCam;

    public GameObject bossCamParent;

    private GameObject bossCam;

    public static BossManager instance;

    public int interpolationFramesCount = 60; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
        
    }

    private void Start()
    {
        bossCam = bossCamParent.transform.GetChild(0).gameObject;
    }

  

    void Update()
    {
        if (lerpCam)
        {
            float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

            Vector3 interpolatedPosition = Vector3.Lerp(mainCam.transform.position, bossCamParent.transform.position, interpolationRatio);

            mainCam.transform.position = interpolatedPosition;
            if (mainCam.transform.position == bossCamParent.transform.position)
            {
                mainCam.SetActive(false);
                bossCam.SetActive(true);
            }
            elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Interpolate camera position to boss camera, swap active cameras.
            lerpCam = true;
            //Set bossStart to true
            bossStart = true;
        }
    }

}
