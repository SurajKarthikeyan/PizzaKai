using UnityEngine;
using UnityEngine.Tilemaps;

public class ForkyOven : MonoBehaviour
{
    #region Vars

    [SerializeField] private Generator[] generators;
    [SerializeField] private Alarm alarm;
    [SerializeField] private LayerMask bossLayerMask;
    [SerializeField] private Forky forky;
    [SerializeField] private ConveyorBeltScript conveyorBelt;
    [SerializeField] private Tilemap conveyorTilemap;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int genVulnerableTime;


    private bool destroyedOil = false;

    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the player enters the oven, kill the player
        if(collision.gameObject.CompareTag("Player"))
        {
            //There doesnt appear to be a way to get player maxhealth so I just picked a big number
            collision.gameObject.GetComponent<Character>().TakeDamage(999999);
        }
        else if (collision.gameObject.CompareTag("BossForky"))
        {
            print("AfterForkyDeath");
            forky.HelpAfterForky();
            Destroy(collision.gameObject);
        }
        else
        {

            if(collision.gameObject.GetComponent<OilBarrel>() != null)
            {
                if (!destroyedOil && forky.active)
                {
                    destroyedOil = true;
                    conveyorTilemap.animationFrameRate = 0;
                    forky.spawning = false;
                    forky.active = false;
                    forky.StopAllCoroutines();
                    forky.actionTaken = false;
                    Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 25, enemyLayer);
                    for (int i = 0; i < enemyColliders.Length; i++)
                    {
                        Destroy(enemyColliders[i].gameObject);
                    }
                    conveyorBelt.conveyorSpeed = 0;
                    foreach (Generator generator in generators)
                    {
                        if (generator != null)
                        {
                            genVulnerableTime = generator.vulnerableTime;
                            generator.vulnerableTime = 999999;
                        }
                    }
                    WeakenGenerators();
                    DialogueManager.Instance.StopPlayer(true);
                    DialogueManager.Instance.CallDialogueBlock("Boss Hint Block");
                }
                else
                {
                    WeakenGenerators();
                }
                
            }
            Destroy(collision.gameObject);
        }

    }
    public void RestartConveyor()
    {
        conveyorTilemap.animationFrameRate = 1;
        conveyorBelt.conveyorSpeed = -1;
        DialogueManager.Instance.StopPlayer(false);
        forky.spawning = true;
        forky.active = true;
        foreach (Generator generator in generators)
        {
            if (generator != null)
            {
                generator.vulnerableTime = genVulnerableTime;
                
            }
        }
    }

    public void WeakenGenerators()
    {
        foreach (Generator generator in generators)
        {
            if (generator != null)
            {
                alarm.AlarmSystem();
                generator.SetVulnerability();
            }
        }
    }
}
