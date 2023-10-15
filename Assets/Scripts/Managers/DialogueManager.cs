using Fungus;
using System.Collections;
using System.Collections.Generic;
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

    //WHEN MANIPULATING/ACCESSING TRANSFORM OF SAY DIALOG DO IT THROUGH ITS PANEL
    public Transform saydialog;

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
        saydialog.transform.position += new Vector3(1, 0) * Time.deltaTime;
    }

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

    public void PedestrianDialogueMove()
    {
        SayDialog.ActiveSayDialog.transform.position += new Vector3(0.1f, 0.1f);
    }
}
