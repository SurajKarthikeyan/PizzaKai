using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        //this is so it gets set back to zero and won't move until the fight starts
        forky.tilemap.animationFrameRate = 0;
    }
    
    public void ForkyBossStart()
    {
        forky.active = true;
        forky.spawning = true;
        forky.tilemap.animationFrameRate = 1;
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
