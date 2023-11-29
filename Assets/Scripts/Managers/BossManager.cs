using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages bosses, in this particular case Forky
/// </summary>
public class BossManager : MonoBehaviour
{
    public bool bossStart = false;

    public GameObject mainCam;

    public GameObject bossCamParent;

    public ConveyorBeltScript conveyorBelt;

    public Forky forky;

    public AudioSource music;

    public AudioClip bossTheme;

    [ReadOnly]
    public int bulletsReflected;
    
    [SerializeField]
    private List<BoxCollider2D> generatorColliders;

    public static BossManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if ( bulletsReflected > 0 && bulletsReflected % 15 == 0)
        {
            //Play some dialogue of Forky saying that the player can't shoot him or something
            DialogueManager.Instance.CallDialogueBlock("Forky Invincible Block");
        }
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
