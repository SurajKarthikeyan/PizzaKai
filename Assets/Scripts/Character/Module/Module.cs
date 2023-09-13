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

    #region Methods
    #region Public Methods
    public void LinkToMaster(Character master)
    {
        Master = master;
        OnLinked();
    }

    /// <summary>
    /// Called once <see cref="Master"/> is set.
    /// </summary>
    protected virtual void OnLinked() { }
    #endregion

    #region Helper Methods
    protected virtual void OnTransformParentChanged()
    {
        // Check if we are in a character now.
        foreach (var parent in transform.Parents())
        {
            if (parent.HasComponent(out Character character))
            {
                character.AddModule(this);
                return;
            }
        }
    }
    #endregion
    #endregion
}