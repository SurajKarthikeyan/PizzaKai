using System.Collections;
using UnityEngine;

/// <summary>
/// Like Selector<T>, but only allows for one selection.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class MonoSelector<T>
{
    [Tooltip("The elements to select from.")]
    public SelectorElement<T>[] elements;

    public bool Empty => elements.Length == 0;

    public T DoSelection()
    {
        return elements.RandomSelectOne(x => x.probability).selection;
    }
}