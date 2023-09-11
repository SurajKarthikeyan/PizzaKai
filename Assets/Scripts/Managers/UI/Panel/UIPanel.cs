using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to a(n) 
    /// <see cref="UIPanel"/> instance.
    /// </summary>
    private static UIPanel instance;

    /// <inheritdoc cref="instance"/>
    public static UIPanel Instance => instance;
    #endregion

    protected virtual void Awake()
    {
        this.InstantiateSingleton(ref instance);

        if (!panel)
        {
            panel = transform.GetChild(0);
        }
    }

    #region Variables
    public Transform panel;

    private bool active;
    #endregion

    #region Properties
    public bool Active
    {
        get => active;
        set
        {
            if (active != value)
            {
                // Only do things when value is changed.
                active = value;

                panel.gameObject.SetActive(active);

                if (active)
                {
                    // Make this panel appear in front of others.
                    transform.SetAsLastSibling();
                }
            }
        }
    }
    #endregion
}