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

    //WHEN MANIPULATING/ACCESSING TRANSFORM OF SAY DIALOG DO IT THROUGH ITS PANEL
    public Transform saydialog;

    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
            levelFlowchart.ExecuteBlock(blockName);
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning(e);
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