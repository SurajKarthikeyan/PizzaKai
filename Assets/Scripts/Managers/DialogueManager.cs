using Cinemachine;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to a UIManager instance.
    /// </summary>
    private static DialogueManager instance;

    /// <summary>
    /// The static reference to a UIManager instance.
    /// </summary>
    public static DialogueManager Instance => instance;
    #endregion

    #region Variables
    public Flowchart levelFlowchart;

    public CharacterMovementModule player;

    public WeaponMasterModule weaponMasterModule;

    public TextMeshProUGUI toolTipText;

    public Image toolTipImage;

    public List<GameObject> enemyList = new List<GameObject>();

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopPlayer(bool stopPlayer)
    {   
        
        if (stopPlayer)
        {
            player.Master.r2d.velocity = new Vector2(0, player.Master.r2d.velocity.y);
            player.characterAnimator.SetFloat("RightLeftMovement", 0f);
            player.enabled = false;
            weaponMasterModule.enabled = false;
        }
        else
        {
            player.enabled = true;
            weaponMasterModule.enabled = true;
            weaponMasterModule.EnableCurrentWeapon();
        }
    }

    public void CallDialogueBlock(string blockName)
    {
        try
        {
            if (levelFlowchart.FindBlock(blockName).State != ExecutionState.Executing)
            {
                levelFlowchart.ExecuteBlock(blockName);
            }
        }
        catch
        {
            Debug.LogError("Block not found");
        }
        
    }

    public void DisableEnemies()
    {
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(player.gameObject.transform.position, 25))
        {
            EnemyBasic enemy = collider.gameObject.GetComponent<EnemyBasic>();
            ChickenMolotov molotov = collider.gameObject.GetComponent<ChickenMolotov>();
            if (enemy != null)
            {
                enemyList.Add(enemy.gameObject);
                enemy.enabled = false;
            }
            else if (molotov != null)
            {
                Destroy(molotov.gameObject);
            }
        }
    }

    public void EnableEnemies()
    {
        List<GameObject> toRemove = new List<GameObject>();
        if(enemyList.Count > 0)
        {
            Debug.Log("Enemies in enemy list");
            foreach(GameObject enemy in enemyList)
            {
                enemy.GetComponent<EnemyBasic>().enabled = true;
                toRemove.Add(enemy);
            }
            foreach (GameObject enemy in toRemove)
            {
                enemyList.Remove(enemy);
            }
        }
    }

    public void SetToolTipImage(string text, Sprite weaponImage)
    {
        toolTipText.text = text;
        toolTipImage.sprite = weaponImage;
    }

    public void ActivateCamera(GameObject camera, bool activateCam)
    {
        camera.GetComponent<CinemachineVirtualCamera>().enabled = activateCam;
    }
}
