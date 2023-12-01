using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to an GameManager instance.
    /// </summary>
    private static GameManager instance;

    /// <summary>
    /// The static reference to an GameManager instance.
    /// </summary>
    public static GameManager Instance => instance;
    #endregion

    #region Variables
    [Tooltip("How long is the invulnerability between taking damage?")]
    public float damageTickRate = 0.5f;

    [Tooltip("The layers where characters can jump off of.")]
    public LayerMask canJumpLayers;

    [Tooltip("The first respawn point for the player. If not set, Respawn " +
        "will use its initial position.")]
    public Transform defaultRespawnPoint;

    [Tooltip("The player character.")]
    [ReadOnly]
    [SerializeField]
    private Character player = null;

    private WeaponMasterModule playerWeaponMaster;

    private CharacterMovementModule playerMovement;
    #endregion

    #region Properties
    /// <summary>
    /// The player character.
    /// </summary>
    public Character Player
    {
        get => player;
        set
        {
            if (player)
            {
                throw new System.InvalidOperationException(
                    "Cannot have multiple players!"
                );
            }
            else
            {
                player = value;
                player.RequireComponentInChildren(out playerWeaponMaster);
                player.RequireComponent(out playerMovement);
            }
        }
    }

    /// <summary>
    /// The <see cref="WeaponMasterModule"/> of the player.
    /// </summary>
    public WeaponMasterModule PlayerWeapons => playerWeaponMaster;

    /// <summary>
    /// The movement module of the player.
    /// </summary>
    public CharacterMovementModule PlayerMovement => playerMovement;
    #endregion

    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }
}