using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class that manages bosses, in this particular case Forky
/// </summary>
public class BossManager : MonoBehaviour
{
    public ConveyorBeltScript conveyorBelt;

    public Forky forky;

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
        if ( bulletsReflected > 0 && bulletsReflected % 15 == 0 && forky.active)
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
        foreach(BoxCollider2D collider in generatorColliders)
        {
            collider.enabled = true;
        }
    }

}
