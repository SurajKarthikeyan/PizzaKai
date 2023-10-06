using UnityEngine;

/// <summary>
/// A module is a plug-in for the character that allows it to do things, such as
/// move and attack. 
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class Module : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// The <see cref="Character"/> that this is attached to.
    /// </summary>
    public Character Master { get; private set; }
    #endregion

    #region Public Methods
    public void LinkToMaster(Character master)
    {
        Master = master;
        OnLinked();
    }

    /// <summary>
    /// Called once <see cref="Master"/> is set from <see
    /// cref="LinkToMaster(Character)"/>.
    /// </summary>
    protected virtual void OnLinked() { }
    #endregion
}