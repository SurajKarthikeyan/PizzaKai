using Fungus;
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

    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
    }
    #endregion

    #region Variables
    public Flowchart levelFlowchart;

    private WeaponMasterModule weaponMasterModule;
    #endregion

    #region Properties
    private Character Player => GameManager.Instance.Player;
    #endregion

    private void Start()
    {
        Player.gameObject.RequireComponentInChildren(out weaponMasterModule);
    }

    public void StopPlayer(bool stopPlayer)
    {   
        
        if (stopPlayer)
        {
            Player.r2d.velocity = new Vector2(0, 0);
            Player.enabled = false;
            weaponMasterModule.enabled = false;
        }
        else
        {
            Player.enabled = true;
            weaponMasterModule.enabled = true;
            weaponMasterModule.EnableCurrentWeapon();
        }
    }

    public void CallDialogueBlock(string blockName)
    {
        levelFlowchart.ExecuteBlock(blockName);
    }
}
