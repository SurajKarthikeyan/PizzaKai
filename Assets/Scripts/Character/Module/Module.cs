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

    private void OnTransformParentChanged()
    {
        if (!Master || !transform.IsChildOf(Master.transform))
        {
            // New master. Remove module from old.
            if (Master)
                Master.RemoveModule(this);

            if (this.HasComponentInAnyParent(out Character m))
            {
                m.AddModule(this);
            }
        }
    }

    private void OnDestroy()
    {
        if (Master)
        {
            Master.RemoveModule(this);
        }
    }

    #region Public Methods
    public void LinkToMaster(Character master)
    {
        if (master != Master)
        {
            var old = Master;
            Master = master;
            OnLinked(old); 
        }
    }

    /// <summary>
    /// Called once <see cref="Master"/> is set from <see
    /// cref="LinkToMaster(Character)"/>.
    /// </summary>
    protected virtual void OnLinked(Character oldMaster) { }
    #endregion
}