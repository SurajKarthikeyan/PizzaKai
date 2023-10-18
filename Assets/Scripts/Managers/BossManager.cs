using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public bool bossStart = false;

    private bool lerpCam = false;

    public GameObject mainCam;

    public GameObject bossCamParent;

    private GameObject bossCam;

    public ConveyorBeltScript conveyorBelt;

    public Forky forky;

    public AudioSource music;

    public AudioClip bossTheme;
    
    [SerializeField]
    private List<BoxCollider2D> generatorColliders;

    public static BossManager instance;

    public int interpolationFramesCount = 60; // Number of frames to completely interpolate between the 2 positions
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
        //if (lerpCam)
        //{
        //    float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        //    Vector3 interpolatedPosition = Vector3.Lerp(mainCam.transform.position, bossCamParent.transform.position, interpolationRatio);

        //    mainCam.transform.position = interpolatedPosition;
        //    if (mainCam.transform.position == bossCamParent.transform.position)
        //    {
        //        mainCam.SetActive(false);
        //        bossCam.SetActive(true);
        //    }
        //    elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
        //}
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Interpolate camera position to boss camera, swap active cameras.
            //lerpCam = true;
            mainCam.SetActive(false);
            bossCamParent.SetActive(true);
            //Set bossStart to true
            bossStart = true;
        }
    }

    public void ForkyBossStart()
    {
        forky.active = true;
        conveyorBelt.conveyorSpeed = -1;
        music.Stop();
        music.PlaySound(bossTheme, 1);
        music.loop = true;
        foreach(BoxCollider2D collider in generatorColliders)
        {
            collider.enabled = true;
        }
    }

}
