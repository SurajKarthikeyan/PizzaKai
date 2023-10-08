using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector2 maxPlayerMoveSpeed;

    private List<WeaponModule> playerWeapons;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        maxPlayerMoveSpeed = player.maxMoveSpeed;
        playerWeapons = weaponMasterModule.weapons;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void StopPlayer(bool stopPlayer)
    {   
        
        if (stopPlayer)
        {
            player.Master.r2d.velocity = new Vector2(0, 0);
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
        levelFlowchart.ExecuteBlock(blockName);
    }
}
