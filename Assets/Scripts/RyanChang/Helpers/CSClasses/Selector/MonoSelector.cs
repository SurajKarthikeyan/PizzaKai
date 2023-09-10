using UnityEngine;

/// <summary>
/// Randomly selects one component.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class MonoSelector<T>
{
    [Tooltip("The elements to select from.")]
    [SerializeField]
    private SelectorElement<T>[] elements;

    public T DoSelection()
    {
        return elements.RandomSelectOne(x => x.probability).selection;
    }
}