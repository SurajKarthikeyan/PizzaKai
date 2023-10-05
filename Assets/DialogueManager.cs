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
    public CharacterMovementModule player;

    public WeaponMasterModule weaponMasterModule;

    Vector2 maxPlayerMoveSpeed;

    private List<WeaponModule> playerWeapons;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
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
            player.maxMoveSpeed = new Vector2(0, 0);
            foreach (var weapon in weaponMasterModule.weapons)
            {
                weapon.gameObject.SetActive(false);
            }
        }
        else
        {
            player.maxMoveSpeed = maxPlayerMoveSpeed;
            weaponMasterModule.weapons = playerWeapons;
            weaponMasterModule.EnableCurrentWeapon();
        }
    }
}
