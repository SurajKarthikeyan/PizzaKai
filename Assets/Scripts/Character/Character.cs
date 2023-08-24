using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// A character is an enemy or a player, and contains references to variables
/// used by either. This consolidates references and makes further development
/// much more easier and less bug prone.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(DefaultFlipModule))]
public class Character : MonoBehaviour
{
    #region Variables
    #region User Settings
    /// <summary>
    /// The current hp of the character. Please use <see cref="HP"/> instead, as
    /// that fires the <see cref="EventManager.onCharacterDeath"/> event.
    /// </summary>
    private int hp;

    /// <summary>
    /// The maximum health of the character.
    /// </summary>
    public int maxHP;
    #endregion

    #region Autogenerated
    [ReadOnly]
    public Rigidbody2D r2d;

    [ReadOnly]
    public Collider2D c2d;

    [ReadOnly]
    public DefaultFlipModule flipModule;

    private Duration damageInvulnerability;
    #endregion

    #endregion

    #region Properties
    /// <summary>
    /// Determines whether or not the character is a player based on the
    /// existence of a <see cref="PlayerMoveModule"/>.
    /// </summary>
    public bool IsPlayer { get; private set; }

    /// <summary>
    /// The current HP of the character. Setting this value also calls <see
    /// cref="EventManager.onCharacterDeath"/> if the character's health drops
    /// below 0.
    /// </summary>
    public int HP
    {
        get => hp;
        set
        {
            hp = value;

            if (hp <= 0)
            {
                hp = 0;
                EventManager.Instance.onCharacterDeath.Invoke(this);
            }
        }
    }

    public List<Module> Modules { get; private set; } = new();
    #endregion

    #region Methods
    #region Instantiation
    private void Awake()
    {
        SetVars();

        damageInvulnerability = new(GameManager.Instance.damageTickRate);
    }

    // private void OnValidate()
    // {
    //     SetVars();
    // }

    private void SetVars()
    {
        GetComponentsInChildren<Module>(true, Modules);
        Modules.ForEach(module => module.LinkToMaster(this));

        this.RequireComponent(out r2d);
        this.RequireComponent(out c2d);

        this.RequireComponent(out flipModule);

        IsPlayer = this.HasComponentInChildren<PlayerControlModule>();
    }
    #endregion

    #region Main Loop

    #endregion

    #region MonoBehavior Methods
    private void Update()
    {
        // Required to get this to work properly.
        damageInvulnerability.IncrementUpdate();
    }
    #endregion

    #region Flipping Methods
    /// <inheritdoc cref="DefaultFlipModule.SetFacingAngle(float)"/>
    public void SetLookAngle(float theta)
    {
        flipModule.SetFacingAngle(theta);
    }
    #endregion

    #region Health Methods
    /// <summary>
    /// Take <paramref name="damage"/> amount of damage.
    /// </summary>
    /// <param name="damage">How much damage to take.</param>
    /// <returns>True if damage is successfully applied, false otherwise (such
    /// as when the character is invulnerable).</returns>
    public bool TakeDamage(int damage)
    {
        if (damageInvulnerability.IsDone)
        {
            HP -= damage;
            return true;
        }

        return false;
    }
    #endregion
    #endregion
}