using UnityEngine;

/// <summary>
/// An element in selector.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public struct SelectorElement<T>
{
    [Tooltip("The thing that could be chosen.")]
    public T selection;

    [Tooltip("The likelihood that this element will be selected. " +
        "This is not the actual mathematical probability. " +
        "The higher this value, the more likely it is to be selected. " +
        "Elements with probabilities of zero or less MAY be selected IF " +
        "no other elements exists with probabilities of 1 or greater, based " +
        "on the selection mode.")]
    public float probability;
}