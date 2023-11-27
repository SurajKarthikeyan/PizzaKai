using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Randomly selects multiple components.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class MultiSelector<T>
{
    #region Variables
    [Tooltip("The elements to select from.")]
    [SerializeField]
    private SelectorElement<T>[] elements;
    #endregion

    #region Methods
    public IEnumerable<T> DoSelection()
    {
        return elements.RandomSelectMany(x => x.probability)
            .Select(s => s.selection);
    }
    #endregion
}