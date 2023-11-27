using Cinemachine;
using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The manager controlling the Fungus dialog system.
/// </summary>
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

    public TextMeshProUGUI toolTipText;

    public Image toolTipImage;

    //WHEN MANIPULATING/ACCESSING TRANSFORM OF SAY DIALOG DO IT THROUGH ITS PANEL
    public Transform saydialog;
    #endregion


    #region Properties
    private Character Player => GameManager.Instance.Player;
    #endregion


    public void StopPlayer(bool stopPlayer)
    {
        var weaponMasterModule = GameManager.Instance.PlayerWeapons;
        if (stopPlayer)
        {
            Player.r2d.velocity = new Vector2(0, Player.r2d.velocity.y);
            Player.characterAnimator.SetFloat("RightLeftMovement", 0f);
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
